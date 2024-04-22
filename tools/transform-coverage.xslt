<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="3.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:b="http://microsoft.com/schemas/VisualStudio/TeamTest/2010">
  <xsl:param name="solutionFolder" />
  <xsl:param name="projectName" />

  <xsl:output method="xml" indent="yes" />
  <xsl:template match="/">
    <coverage version="1">
      <xsl:for-each-group select="//module/functions/function" group-by="@type_name">
        <xsl:variable name="fileName" select="concat(current-grouping-key(), '.cs')" />
        <file path="{$fileName}">
          <xsl:for-each select="//module/functions/function[@type_name=current-grouping-key()]/ranges/range">
            <xsl:variable name="lineNo" select="current()/@start_line"/>
            <xsl:variable name="covered" select="current()/@covered"/>
              <xsl:choose>
                <xsl:when test="$covered = 'yes'">
                  <lineToCover lineNumber="{$lineNo}" covered="true"/>
                </xsl:when>
                <xsl:otherwise>
                  <lineToCover lineNumber="{$lineNo}" covered="false"/>
                </xsl:otherwise>
              </xsl:choose>
          </xsl:for-each>
        </file>
      </xsl:for-each-group>
    </coverage>
  </xsl:template>

</xsl:stylesheet>