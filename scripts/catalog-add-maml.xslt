<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:msxsl="urn:schemas-microsoft-com:xslt"
                xmlns:c="http://schemas.microsoft.com/xsd/catalog"
                exclude-result-prefixes="msxsl c"
>
  <xsl:output method="xml" indent="yes"/>

  <xsl:template match="@* | node()">
    <xsl:copy>
      <xsl:apply-templates select="@* | node()"/>
    </xsl:copy>
  </xsl:template>

  <xsl:template match="/c:SchemaCatalog">
    <SchemaCatalog xmlns="http://schemas.microsoft.com/xsd/catalog">
      <xsl:apply-templates select="@* | node()" />
      <xsl:if test="not(c:Catalog[@href='MAML\catalog.xml'])">
        <xsl:call-template name="AddCatalog" />
      </xsl:if>
    </SchemaCatalog>
  </xsl:template>

  <xsl:template name="AddCatalog">
    <xsl:element name="Catalog" namespace="http://schemas.microsoft.com/xsd/catalog">
      <xsl:attribute name="href">MAML\catalog.xml</xsl:attribute>
    </xsl:element>
  </xsl:template>
</xsl:stylesheet>
