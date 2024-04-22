<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="3.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <xsl:output method="xml" indent="yes" />	
  <xsl:template match="/">
    <testExecutions version="1">
      <xsl:for-each-group select="//test-run//test-case/ancestor::test-suite[last()]" group-by="@name">
        <xsl:variable name="a" select="current-grouping-key()" />
        <file path="{$a}">
          <xsl:for-each select="//test-run/test-suite[@name=$a]//test-case">
            <xsl:variable name="caseName" select="current()/@fullname" />
            <xsl:variable name="caseDuration" select="round(number(current()/@duration) * 1000)" />
            <xsl:variable name="testResult" select="current()/@result" />
            <testCase name="{$caseName}" duration="{$caseDuration}">
              <xsl:choose>
                <xsl:when test="$testResult = 'Failed'">
                  <failure message="Failed"><xsl:value-of select="current()/failure/message"/></failure>
                </xsl:when>
                <xsl:when test="$testResult = 'Skipped'">
                  <skipped message="Ignorred"><xsl:value-of select="current()/reason/message"/></skipped>
                </xsl:when>
                <xsl:when test="$testResult = 'Warning'">
                  <error message="Warning"><xsl:value-of select="current()/reason/message"/></error>
                </xsl:when>
                <xsl:when test="$testResult = 'Inconclusive'">
                  <error message="Inconclusive"><xsl:value-of select="current()/reason/message"/></error>
                </xsl:when>
              </xsl:choose>
            </testCase>
          </xsl:for-each>
        </file>
      </xsl:for-each-group>
    </testExecutions>
  </xsl:template>

</xsl:stylesheet>