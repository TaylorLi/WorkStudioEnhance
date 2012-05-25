<?xml version="1.0"?>
<Settings xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <ProjectName>Adholidays.H</ProjectName>
  <ConnectionKey>Config.ConnectionKeys.Adholidays</ConnectionKey>
  <DataObjectNameSpace>TestLibrary.DB.Adholidays.DAL</DataObjectNameSpace>
  <BusinessObjectNameSpace>TestLibrary.DB.Adholidays.BAL</BusinessObjectNameSpace>
  <ConnectionString>Data Source=.;Initial Catalog=Adholidays;Persist Security Info=True;User ID=sa;Password=gz123;Connect Timeout=30</ConnectionString>
  <UnInsertAndUnUpdate>CreateOn</UnInsertAndUnUpdate>
  <UnInsert>UpdateOn,UpdateBy</UnInsert>
  <UnUpdate>CreateBy</UnUpdate>
  <DOPrefix>DO_</DOPrefix>
  <UOPrefix>UO_</UOPrefix>
  <UOListPrefix>UOList_</UOListPrefix>
  <BOPrefix>BO_</BOPrefix>
  <IsPassConnectionStringToBusiness>false</IsPassConnectionStringToBusiness>
</Settings>