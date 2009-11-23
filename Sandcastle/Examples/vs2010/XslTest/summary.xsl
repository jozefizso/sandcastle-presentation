<?xml version="1.0" encoding="UTF-8"?>
<?altova_samplexml file:///D:/dev/sc-vs2010/Sandcastle/Examples/vs2010/XslTest/document.xml?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:fo="http://www.w3.org/1999/XSL/Format">

	<xsl:output indent="yes" method="xml"/>
	
	<xsl:template match="/document">
		<doc>
			<xsl:apply-templates/>
		</doc>
	</xsl:template>

	<xsl:template match="summary">
		<div class="summary">
			<xsl:apply-templates/>
		</div>
	</xsl:template>

	<xsl:template match="summary/text()">
		<test>
			<xsl:value-of select="normalize-space(.)"/>
		</test>
	</xsl:template>

	<xsl:template match="para">
		<p>
			<xsl:apply-templates/>
		</p>
	</xsl:template>
</xsl:stylesheet>
