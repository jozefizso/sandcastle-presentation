<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0" 
				xmlns:MSHelp="http://msdn.microsoft.com/mshelp"
        xmlns:mshelp="http://msdn.microsoft.com/mshelp"
				xmlns:ddue="http://ddue.schemas.microsoft.com/authoring/2003/5"
				xmlns:xlink="http://www.w3.org/1999/xlink"
        xmlns:msxsl="urn:schemas-microsoft-com:xslt"
         >

  <xsl:import href="utilities_reference.xsl"/>

  <xsl:template name="body">
    <xsl:call-template name="topic" />
    <xsl:call-template name="bodyMain" />
  </xsl:template>

  <xsl:template name="topic">
    <div class="OH_topic">
      <h1 class="OH_title">
        <include item="nsrTitle">
          <parameter>
            <xsl:call-template name="topicTitleDecorated"/>
          </parameter>
        </include>
      </h1>
      <p class="topicMetaVersion">
        This page is specific to: <strong>.NET Framework Version:</strong>
        2.0 3.0 <strong>3.5</strong> 4.0
      </p>
    </div>
  </xsl:template>

  <xsl:template name="bodyMain">
    <div id="mainSection">
      <div id="mainBody">
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

      </div>
    </div>
  </xsl:template>

  <xsl:template name="bodySummary">
    <!-- enclosing <div>s will be created by actual XSL templates -->
    <xsl:apply-templates select="/document/comments/preliminary" />
    <xsl:apply-templates select="/document/comments/summary" />
  </xsl:template>
  
  
</xsl:stylesheet>