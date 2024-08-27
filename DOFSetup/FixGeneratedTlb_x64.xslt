<!--
Summary:
   Fix up the heat-generated RegisterDirectOutputComObjectTlb.wxs file for x64.
   
How this file is invoked:
   This is invoked by the Templates="..." attribute in the <HeatFile> section
   in the .wixproj file.  Note that this doesn't show up anywhere in the Visual
   Studio GUI - you have to hand-edit the .wixproj file to see it or change it.

What this file does:
   Heat has some known bugs for x64, and the v3 tool is no longer maintained, so
   they're never going to get fixed.  The recommended workaround seems to be
   pretty awful: use XSL to rewrite the generated file to fix the problems.

   1. Heat places <TypeLib> in the <Component> section, but the Wix compiler
   requires it to be under the <File> section.  The XSL relocates <TypeLib>
   accordingly.

   2. The generated <TypeLib> lacks a "Language" attribute.  On x86, this
   just generates a Wix compiler warning; on x64, it's a hard error.  The
   XSL adds a 'Language="0"' attribute to the <TypeLib>.

   3. The generated file contains a <RegistryValue> that's redundant with
   the one implied by the <File><TypeLib> entry.  The XSL removes it.
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

   <!-- Insert a copy of the Component/TypeLib node under File -->
   <xsl:template match="wix:File">
      <xsl:copy>
         <xsl:apply-templates select="@*" />
         <xsl:apply-templates select="node()" />

         <!-- add attribute 'Library="0"' to the relocated TypeLib node -->
         <wix:TypeLib>
            <xsl:apply-templates select="../wix:TypeLib/@*" />
            <xsl:attribute name="Language"><xsl:value-of select="0" /></xsl:attribute>
            <xsl:copy-of select="../wix:TypeLib/node()" />
         </wix:TypeLib>
      </xsl:copy>
   </xsl:template>

   <!-- Remove the original Component/TypeLib node -->
   <xsl:template match="//wix:Component/wix:TypeLib" />

   <!-- Remove the original Component/RegistryValue node -->
   <xsl:template match="//wix:Component/wix:RegistryValue" />

</xsl:stylesheet>
