<!--
Summary:
   Fix up the heat-generated RegisterDirectOutputComObjectTlb.wxs file for x86.
   No changes are required for x86, the build process requires an .xslt file,
   so this file fills the role by providing an identity transform: i.e., just
   pass the original heat.exe output unchanged.
   
How this file is invoked:
   This is invoked by the Templates="..." attribute in the <HeatFile> section
   in the .wixproj file.  Note that this doesn't show up anywhere in the Visual
   Studio GUI - you have to hand-edit the .wixproj file to see it or change it.

What this file does:
   Nothing; it's just an identity transform, since no changes are required for
   the heat.exe output on x86.  (Why have the file at all, then?  Because the
   build process requires one for each platform.)
-->

<xsl:stylesheet version="1.0" 
  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
  xmlns:wix="http://schemas.microsoft.com/wix/2006/wi" >

  <!-- Indentation in XSL -->
  <xsl:output method="xml" version="1.0" omit-xml-declaration="yes" encoding="UTF-8" indent="yes"/>

  <!-- Identity rule -->
  <xsl:template match="@*|node()">
    <xsl:copy>
      <xsl:apply-templates select="@*|node()" />
    </xsl:copy>
  </xsl:template>

</xsl:stylesheet>
