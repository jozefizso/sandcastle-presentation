<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet
  version="1.0"
  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
  xmlns:msxsl="urn:schemas-microsoft-com:xslt"
  xmlns:se="http://schemas.izsak.net/sandcastle/2010/extensions"
  exclude-result-prefixes="msxsl se"
>
  <!--
  <xsl:output method="html" indent="yes"
                doctype-system="about:legacy-compat"/>

  -->
  <xsl:template match="se:imHistory">
    <html>
      <head>
        <title>Miranda History</title>
        <link rel="stylesheet" href="miranda.css" type="text/css" />
      </head>
      <body>

        <div class="imhistory">
          <xsl:apply-templates />
        </div>
      </body>
    </html>
  </xsl:template>

  <xsl:template match="se:title">
    <h3 class="imhistory-title">
      <xsl:apply-templates/>
    </h3>
  </xsl:template>

  <xsl:template match="IMHISTORY">
    <div class="imhistory-content">
      <xsl:call-template name="dateTime" />
      <xsl:apply-templates />
    </div>
  </xsl:template>
  
  <xsl:template match="EVENT">
    <div>
      <xsl:if test="not(FROM = preceding::EVENT[1]/FROM)">
        <strong>
          <xsl:value-of select="FROM"/>
        </strong>
        <xsl:text> </xsl:text>
        <xsl:call-template name="inOut">
          <xsl:with-param name="contact" select="CONTACT" />
          <xsl:with-param name="from" select="FROM" />
        </xsl:call-template>
      </xsl:if>

      <xsl:text> </xsl:text>
      <xsl:apply-templates select="MESSAGE" />
    </div>
  </xsl:template>

  <xsl:template match="MESSAGE">
    <xsl:call-template name="replace">
      <xsl:with-param name="string" select="."/>
    </xsl:call-template>
  </xsl:template>

  <xsl:template name="replace">
    <xsl:param name="string"/>
    <xsl:choose>
      <xsl:when test="contains($string,'&#10;')">
        <xsl:value-of select="substring-before($string,'&#10;')"/>
        <br/>
        <xsl:call-template name="replace">
          <xsl:with-param name="string" select="substring-after($string,'&#10;')"/>
          </xsl:call-template>
      </xsl:when>
      <xsl:otherwise>
        <xsl:value-of select="$string"/>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template name="dateTime">
    <p>
      <xsl:value-of select="EVENT[1]/DATE"/>
    </p>      
  </xsl:template>
  
  
  <xsl:template name="inOut">
    <xsl:param name="contact"/>
    <xsl:param name="from"/>

    <!--
    <xsl:variable name="imageName" select="'outgoing'" />
    <xsl:choose>
      <xsl:when test="$contact = $from">
        <!- - incoming message - ->
        &#0171;
      </xsl:when>
      <xsl:otherwise>
        <!- - outgoing message - ->
        &#0187;
      </xsl:otherwise>
    </xsl:choose>
    -->

    <xsl:call-template name="inOutImage">
      <xsl:with-param name="imageName">
        <xsl:choose>
          <xsl:when test="$contact = $from">
            <xsl:text>incoming</xsl:text>
          </xsl:when>
          <xsl:otherwise>
            <xsl:text>outgoing</xsl:text>
          </xsl:otherwise>
        </xsl:choose>
      </xsl:with-param>
    </xsl:call-template>
  </xsl:template>

  <xsl:template name="inOutImage">
    <xsl:param name="imageName"/>


    <img alt="">
      <includeAttribute name="src" item="imagePath">
        <parameter>
          <xsl:text>msg-</xsl:text>
          <xsl:value-of select="$imageName"/>
          <xsl:text>.gif</xsl:text>
        </parameter>
      </includeAttribute>
    </img>
    
    <!--<xsl:element name="img">
      <xsl:attribute name="src">
        <xsl:text>msg-</xsl:text>
        <xsl:value-of select="$imageName"/>
        <xsl:text>.gif</xsl:text>
      </xsl:attribute>
      <xsl:attribute name="alt">
        <xsl:text></xsl:text>
      </xsl:attribute>
    </xsl:element>-->
    
  </xsl:template>
</xsl:stylesheet>
