<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet
  version="1.0"
  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
  xmlns:msxsl="urn:schemas-microsoft-com:xslt"
  xmlns:bib="http://sandcastle.codeplex.com/schemas/2010/biblio"
  exclude-result-prefixes="msxsl"
>
  <xsl:template match="bib:book">
    <p>
      <xsl:call-template name="biblio_stn_iso690" />
    </p>
  </xsl:template>

  <xsl:template name="biblio_stn_iso690">
    <xsl:value-of select="bib:author" />
    <xsl:text>. </xsl:text>
    <em>
      <xsl:value-of select="bib:title"/>
    </em>
    <xsl:text>. </xsl:text>
    <xsl:value-of select="bib:publisher"/>
    <xsl:text>, </xsl:text>
    <xsl:value-of select="bib:year"/>
    <xsl:text>. </xsl:text>
    <xsl:apply-templates select="bib:isbn" />
  </xsl:template>

  <xsl:template match="bib:isbn">
    <xsl:text>ISBN </xsl:text>
    <xsl:value-of select="."/>
    <xsl:text>.</xsl:text>
  </xsl:template>
</xsl:stylesheet>
