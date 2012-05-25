--Sample Page Select
  /*
  SELECT * FROM 
  (
  SELECT A.*, ROWNUM RUWNUMBER 
  FROM (SELECT * FROM TABLE_NAME) A 
  WHERE ROWNUM <= 40
  )
  WHERE RUWNUMBER >= 21
  */
	--Paging Store Proc
create or replace procedure SP_PAGING(p_TableName   VARCHAR2, --Table name  Eg:T_User
                                      p_PrimaryKeys varchar2, --Primary Keys  Eg:'UserID'
                                      p_PageIndex   INTEGER := 1, --Page index  Eg:1
                                      p_PageSize    INTEGER := 10, --Page size    Eg:10
                                      p_FieldsShow  varchar2 := '', --Show fields  Eg:'*' or 'CompanyId,UserName'
                                      p_FieldsOrder varchar2 := '', --Order fields  Eg:'UserName' or 'CompanyId desc,UserName'
                                      p_Where       varchar2 := '', --Condition    Eg:'CompanyId=''400'' and Status=1'                                      
                                      Result        out SYS_REFCURSOR, --  The Specific Page Result  
                                      Total         out INTEGER -- Total Record Count     
                                      ) is
  v_Sql         VARCHAR2(4000);
  v_QueryType   INT; --1 Table or View,0 SQL Query String 
  v_TableName   VARCHAR2(2000) := p_TableName; --Table name  Eg:T_User
  v_PrimaryKeys varchar2(100) := p_PrimaryKeys; --Primary Keys  Eg:'UserID'
  v_PageIndex   INTEGER := p_PageIndex; --Page index  Eg:1
  v_PageSize    INTEGER := p_PageSize; --Page size    Eg:10
  v_FieldsShow  varchar2(1000) := p_FieldsShow; --Show fields  Eg:'*' or 'CompanyId,UserName'
  v_FieldsOrder varchar2(550) := p_FieldsOrder; --Order fields  Eg:'UserName' or 'CompanyId desc,UserName'
  v_Where       varchar2(2000) := p_Where; --Condition    Eg:''where CompanyCode=''HKG13'' and TeamCode=''GAC'''
  v_TopLow      VARCHAR2(20);
  v_TopHigh     VARCHAR2(20);
  v_SqlTemp     VARCHAR2(4000);
begin
  if length(v_TableName) is null then
    RAISE_APPLICATION_ERROR(-20100, 'p_TableName can not be empty.');
  end if;
  -- Check Query Type
  select COUNT(1)
    into v_QueryType
    from ALL_TABLES tall
   where upper(tall.TABLE_NAME) = upper(v_TableName);
  --Check parameters
  if v_PageIndex is null or v_PageIndex < 1 THEN
    v_PageIndex := 1;
  end if;
  IF v_PageSize is null or v_PageSize < 1 THEN
    v_PageSize := 10;
  end if;
  IF length(v_FieldsShow) is null THEN
    v_FieldsShow := '*';
  end if;
  IF length(v_FieldsOrder) is null THEN
    v_FieldsOrder := '';
  ELSE
    v_FieldsOrder := 'ORDER BY ' || LTRIM(v_FieldsOrder);
  end if;
  IF length(v_Where) is null THEN
    v_Where := '';
  ELSE
    v_Where := 'WHERE ' || v_Where || '';
  end if;

  --Get Total Result Count
  IF Total IS NULL THEN
    BEGIN
      IF v_QueryType = 1 THEN
        v_Sql := ' SELECT COUNT(*) ' || ' FROM ' || v_TableName || ' ' ||
                 v_Where;
      ELSE
        v_Sql := ' SELECT COUNT(*) ' || ' FROM (' || v_TableName || ') a ' || ' ' ||
                 v_Where;
      END IF;
      --DBMS_OUTPUT.put_line(v_Sql);
      EXECUTE IMMEDIATE v_Sql
        INTO Total;
    END;
  end if;

  --Paging

  v_TopLow  := to_char((v_PageIndex - 1) * v_PageSize + 1);
  v_TopHigh := to_char(v_PageIndex * v_PageSize);
  -- query for A
  IF v_QueryType = 1 then
    v_SqlTemp := 'SELECT ' || v_FieldsShow || ' FROM ' || v_TableName || ' ' ||
                 v_Where || ' ' || v_FieldsOrder;
  ELSE
    v_SqlTemp := 'SELECT ' || v_FieldsShow || ' FROM (' || v_TableName ||
                 ')  TmpTable ' || v_Where || ' ' || v_FieldsOrder;
  end if;
  --DBMS_OUTPUT.put_line(v_SqlTemp);
  if v_PageIndex = 1 THEN
    /*
    Sample
    
     SELECT TmpTa.*, ROWNUM RUWNUMBER
      FROM (SELECT * FROM TABLE_NAME) TmpTa 
      WHERE ROWNUM <= 10
    */
    BEGIN
      v_sql := 'SELECT TmpTa.*, ROWNUM ROWNUMBER
  FROM (' || v_SqlTemp || ') TmpTa 
  WHERE ROWNUM <= ' || v_TopHigh;
    end;
  ELSE
    BEGIN
      /*
        Sample
        
      SELECT * FROM 
      (
      SELECT TmpTa.*, ROWNUM ROWNUMBER 
      FROM (SELECT * FROM TABLE_NAME) TmpTa 
      WHERE ROWNUM <= 40
      )
      WHERE ROWNUMBER >= 21
      */
      v_sql := 'SELECT * FROM 
		  (
			 SELECT TmpTa.*, ROWNUM RUWNUMBER
       FROM (' || v_SqlTemp || ') TmpTa 
       WHERE ROWNUM <= ' || v_TopHigh || '
			)
	 WHERE RUWNUMBER >= ' || v_TopLow;
    end;
  end if;
  --DBMS_OUTPUT.put_line(v_Sql);
  open Result for(v_Sql);
end SP_PAGING;
