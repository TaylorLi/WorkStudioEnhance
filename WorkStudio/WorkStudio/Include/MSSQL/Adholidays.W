<?xml version="1.0"?>
<Settings xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <ProjectName>Adholidays.W</ProjectName>
  <ConnectionKey>Config.ConnectionKeys.Adholidays</ConnectionKey>
  <DataObjectNameSpace>TestLibrary.DB.Adholidays.DAL</DataObjectNameSpace>
  <BusinessObjectNameSpace>TestLibrary.DB.Adholidays.BAL</BusinessObjectNameSpace>
  <ConnectionString>Data Source=10.100.133.83;Initial Catalog=Adholidays;Persist Security Info=True;User ID=sa;Password=gzuat;Connect Timeout=30</ConnectionString>
  <UnInsertAndUnUpdate>CreateOn</UnInsertAndUnUpdate>
  <UnInsert>UpdateOn,UpdateBy</UnInsert>
  <UnUpdate>CreateBy</UnUpdate>
  <DOPrefix>DO_</DOPrefix>
  <UOPrefix>UO_</UOPrefix>
  <UOListPrefix>UOList_</UOListPrefix>
  <BOPrefix>BO_</BOPrefix>
  <IsPassConnectionStringToBusiness>false</IsPassConnectionStringToBusiness>
</Settings>