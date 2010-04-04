<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0" 
				xmlns:MSHelp="http://msdn.microsoft.com/mshelp"
        xmlns:mshelp="http://msdn.microsoft.com/mshelp"
				xmlns:ddue="http://ddue.schemas.microsoft.com/authoring/2003/5"
				xmlns:xlink="http://www.w3.org/1999/xlink"
        xmlns:msxsl="urn:schemas-microsoft-com:xslt"
         >

  <xsl:import href="html_body_header.xsl"/>
  <xsl:import href="html_body_navigation.xsl"/>
  <xsl:import href="html_body_footer.xsl"/>
  
  <xsl:import href="utilities_reference.xsl"/>

  <!-- main window -->

  <xsl:template name="main">
    <div class="contentPlaceHolder">
      <xsl:call-template name="navigation" />

        <xsl:call-template name="body" />
      <xsl:call-template name="footer" />
    </div>
  </xsl:template>
  
  <xsl:template name="body">
    <div class="content">
      <div class="topicContainer">
        <div class="topic">
          <xsl:call-template name="documentTitle" />
          <xsl:call-template name="bodyMain" />
        </div>
      </div>
    </div>
  </xsl:template>

  <xsl:template name="documentTitle">
    <p class="majorTitle">
      <xsl:call-template name="projectTitle"/>
    </p>
    <h1 class="title">
      <xsl:call-template name="topicTitleDecorated"/>
    </h1>
    <xsl:call-template name="platformInformation" />
  </xsl:template>

  <xsl:template name="bodyMain">
    <div id="mainSection">
      <div id="mainBody">
        <xsl:call-template name="bodyContent" />
      </div>
    </div>
  </xsl:template>

  <xsl:template name="bodyContent">
    <xsl:call-template name="bodySummary" />

    <xsl:if test="$subgroup='overload'">
      <xsl:apply-templates select="/document/reference/elements" mode="overloadSummary" />
    </xsl:if>
    <!-- assembly information -->
    <xsl:if test="not($group='list' or $group='root' or $group='namespace')">
      <xsl:call-template name="requirementsInfo"/>
    </xsl:if>
    <!-- syntax -->
    <xsl:if test="not($group='list' or $group='namespace')">
      <xsl:apply-templates select="/document/syntax" />
    </xsl:if>
    <!-- members -->
    <xsl:choose>
      <xsl:when test="$group='root'">
        <xsl:apply-templates select="/document/reference/elements" mode="root" />
      </xsl:when>
      <xsl:when test="$group='namespace'">
        <xsl:apply-templates select="/document/reference/elements" mode="namespace" />
      </xsl:when>
      <xsl:when test="$subgroup='enumeration'">
        <xsl:apply-templates select="/document/reference/elements" mode="enumeration" />
      </xsl:when>
      <xsl:when test="$group='type'">
        <xsl:apply-templates select="/document/reference/elements" mode="type" />
      </xsl:when>
      <xsl:when test="$group='list'">
        <xsl:choose>
          <xsl:when test="$subgroup='overload'">
            <xsl:apply-templates select="/document/reference/elements" mode="overload" />
          </xsl:when>
          <xsl:when test="$subgroup='DerivedTypeList'">
            <xsl:apply-templates select="/document/reference/elements" mode="derivedType" />
          </xsl:when>
          <xsl:otherwise>
            <xsl:apply-templates select="/document/reference/elements" mode="member" />
          </xsl:otherwise>
        </xsl:choose>
      </xsl:when>
    </xsl:choose>
    <!-- remarks -->
    <xsl:apply-templates select="/document/comments/remarks" />
    <!-- example -->
    <xsl:apply-templates select="/document/comments/example" />
    <!-- other comment sections -->
    <!-- permissions -->
    <xsl:call-template name="permissions" />
    <!-- exceptions -->
    <xsl:call-template name="exceptions" />
    <!-- inheritance -->
    <xsl:apply-templates select="/document/reference/family" />
    <xsl:apply-templates select="/document/comments/threadsafety" />
    <!--versions-->
    <xsl:if test="not($group='list' or $group='namespace' or $group='root' )">
      <xsl:apply-templates select="/document/reference/versions" />
    </xsl:if>
    <!-- see also -->
    <xsl:call-template name="seealso" />

  </xsl:template>

  <xsl:template name="bodySummary">
    <!-- enclosing <div>s will be created by actual XSL templates -->
    <xsl:apply-templates select="/document/comments/preliminary" />
    <xsl:apply-templates select="/document/comments/summary" />
  </xsl:template>

  <xsl:template name="platformInformation">
    <xsl:choose>
      <xsl:when test="/document/metadata/platform">
        <p class="topicMetaVersion">
          <xsl:apply-templates select="/document/metadata/platform" />
        </p>
      </xsl:when>
      <xsl:otherwise>
        <xsl:comment>(No platform information found in metadata.)</xsl:comment>
      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <xsl:template match="metadata/platform">
    <xsl:variable name="name" select="@name" />
    <xsl:variable name="version" select="@version" />
    
    <include item="{$name}PlatformTitle" />
    <strong>
      <xsl:value-of select="@version"/>
    </strong>
  </xsl:template>
  
</xsl:stylesheet>