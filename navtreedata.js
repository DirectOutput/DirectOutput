/*
 @licstart  The following is the entire license notice for the JavaScript code in this file.

 The MIT License (MIT)

 Copyright (C) 1997-2020 by Dimitri van Heesch

 Permission is hereby granted, free of charge, to any person obtaining a copy of this software
 and associated documentation files (the "Software"), to deal in the Software without restriction,
 including without limitation the rights to use, copy, modify, merge, publish, distribute,
 sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is
 furnished to do so, subject to the following conditions:

 The above copyright notice and this permission notice shall be included in all copies or
 substantial portions of the Software.

 THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING
 BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
 DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

 @licend  The above is the entire license notice for the JavaScript code in this file
*/
var NAVTREE =
[
  [ "DirectOutput", "index.html", [
    [ "Welcome to the DirectOutput framework R3 for virtual pinball cabinets", "index.html#autotoc_md11", null ],
    [ "Installation and Configuration", "installation.html", [
      [ "Requirements", "installation.html#installation_requirements", null ],
      [ "Download", "installation.html#installation_download", null ],
      [ "Installation", "installation.html#installation_installation", [
        [ "Installation within hosting application directory structure", "installation.html#installation_installation_hostappdir", null ],
        [ "Installation in own directory (Recommended)", "installation.html#installation_installation_owndir", null ]
      ] ],
      [ "Unblock the DLLs", "installation.html#installation_unblockauc", null ],
      [ "B2S.Server Configuration", "installation.html#installation_b2sserverconfig", null ],
      [ "Visual Pinball core.vbs Adjustment", "installation.html#installation_visualpinballcorevbs", null ],
      [ "Configuration", "installation.html#installation_configuration", [
        [ "Using Auto configuration", "installation.html#installation_autoconfiguration", null ],
        [ "Using directoutputconfig.ini files", "installation.html#installation_ledcontrolini", null ],
        [ "Using configuration files", "installation.html#installation_configfiles", [
          [ "Global Configuration", "installation.html#installation_globalconfig", null ],
          [ "Cabinet Configuration", "installation.html#installation_cabinetconfig", null ]
        ] ]
      ] ]
    ] ],
    [ "VP Table configuration", "tableconfig.html", [
      [ "Introduction", "tableconfig.html#tableconfig_intro", null ],
      [ "VP solid state (SS) tables", "tableconfig.html#tableconfig_VPSS", [
        [ "Configure SS tables", "tableconfig.html#tableconfig_VPSSconfig", null ],
        [ "Extend SS tables", "tableconfig.html#tableconfig_VPSSextend", null ]
      ] ],
      [ "VP electro mechanical (EM) tables and original tables", "tableconfig.html#tableconfig_VPEM", [
        [ "Initialization", "tableconfig.html#tableconfig_VPEMinit", null ],
        [ "Termination", "tableconfig.html#tableconfig_VPEMexit", null ],
        [ "Score commands", "tableconfig.html#tableconfig_VPEMscore", null ],
        [ "Table element status updates", "tableconfig.html#tableconfig_VPEMtableelements", [
          [ "The basics", "tableconfig.html#tableconfig_VPEMtableelements_basics", null ],
          [ "The easy way", "tableconfig.html#tableconfig_VPEMtableelements_easy", null ]
        ] ]
      ] ],
      [ "Tables w/o B2S.Server Backglass", "tableconfig.html#tableconfig_nobackglass", null ]
    ] ],
    [ "PinballX support", "pbx.html", [
      [ "Introduction", "pbx.html#pbx_intro", null ],
      [ "Setup", "pbx.html#pbx_setup", null ],
      [ "Ini File Parameters", "pbx.html#pbx_infilepara", null ]
    ] ],
    [ "Supported Hardware", "hardware.html", [
      [ "Introduction", "hardware.html#hardware_intro", null ],
      [ "LedWiz (GroovyGameGear)", "hardware.html#hardware_ledwiz", null ],
      [ "PacLed64 (Ultimarc)", "hardware.html#hardware_ultimarc_pacled64", null ],
      [ "PacDrive (Ultimarc)", "hardware.html#hardware_ultimarc_pacdrive", null ],
      [ "Art-Net / DMX", "hardware.html#hardware_artnet", null ],
      [ "Teensy Strip Controller", "hardware.html#hardware_TeensyStripController", null ],
      [ "WS2811 addressable LedStrip controller", "hardware.html#hardware_WS2811", null ],
      [ "FT245RL based controllers (e.g. SainSmart)", "hardware.html#hardware_FT245bitbang", null ]
    ] ],
    [ "Global Configuration", "globalconfig2.html", [
      [ "Introduction", "globalconfig2.html#globalconfig_introduction", null ],
      [ "Global config editor", "globalconfig2.html#globalconfig_editor", [
        [ "Ini Files tab", "globalconfig2.html#globalconfig_inifiles", null ],
        [ "Cabinet Config tab", "globalconfig2.html#globalconfig_cabinetconfig", null ],
        [ "Logging tab", "globalconfig2.html#globalconfig_logging", null ],
        [ "Misc tab", "globalconfig2.html#globalconfig_misc", null ]
      ] ],
      [ "file instrumentation", "globalconfig2.html#Log", null ],
      [ "File format", "globalconfig2.html#globalconfig_fileformat", null ]
    ] ],
    [ "Cabinet Configuration", "cabinetconfig.html", [
      [ "Introduction", "cabinetconfig.html#cabinetconfig_introduction", null ],
      [ "Cabinet configuration file", "cabinetconfig.html#cabinetconfig_configfile", [
        [ "OutputControllers section", "cabinetconfig.html#cabinetconfig_outputcontrollers", null ],
        [ "Toys section", "cabinetconfig.html#cabinetconfig_toys", null ]
      ] ],
      [ "Example cabinet configuration", "cabinetconfig.html#cabinetconfig_example", null ]
    ] ],
    [ "Table configuration file", "tableconfigfile.html", [
      [ "Introduction", "tableconfigfile.html#tableconfigfile_introduction", null ],
      [ "Table config file", "tableconfigfile.html#tableconfigfile_configfilestructure", [
        [ "Table elements section", "tableconfigfile.html#tableconfigfile_configfilestructuretableelements", null ],
        [ "Assigned static effects section", "tableconfigfile.html#tableconfigfile_configfilestructurestaticeffects", null ],
        [ "Effects section", "tableconfigfile.html#tableconfigfile_configfilestructureeffects", null ]
      ] ]
    ] ],
    [ "Cabinet Configuration examples", "configexamples.html", [
      [ "Introduction", "configexamples.html#configexamples_introduction", null ],
      [ "Config Examples/Templates", "configexamples.html#configexamples_examples", [
        [ "TeensyStripController example", "configexamples.html#configexamples_teensystripcontroller", null ],
        [ "Fade curve example", "configexamples.html#configexamples_teensystripcontrollerfadecurve", null ]
      ] ],
      [ "User Cabinet configs", "configexamples.html#configexamples_users", [
        [ "Arngrims config", "configexamples.html#configexamples_arngrim", null ],
        [ "Swisslizards config", "configexamples.html#configexamples_swisslizard", null ]
      ] ]
    ] ],
    [ "ini files", "inifiles.html", [
      [ "Introduction", "inifiles.html#inifiles_introduction", null ],
      [ "How it works", "inifiles.html#inifiles_howitworks", null ],
      [ "File locations", "inifiles.html#inifiles_directories", null ],
      [ "LedControl file numbering", "inifiles.html#inifiles_filenumbering", null ],
      [ "Settings in DirectOutputConfig/LedControl ini files", "inifiles.html#inifiles_settings", null ],
      [ "Colors Section", "inifiles.html#inifiles_settingscolors", null ],
      [ "Config DOF Section", "inifiles.html#inifiles_settingsconfigouts", null ],
      [ "Trigger parameters", "inifiles.html#inifiles_triggerpara", null ],
      [ "General parameters", "inifiles.html#inifiles_generalpara", null ],
      [ "Matrix/area effect parameters", "inifiles.html#inifiles_matrix", [
        [ "General Matrix Paras", "inifiles.html#inifiles_matrixeffectpara", null ],
        [ "Shift Effect Paras", "inifiles.html#inifiles_shifteffectpara", null ],
        [ "Flicker Effect Paras", "inifiles.html#inifiles_flickereffectpara", null ],
        [ "Plasma Effect Parameter", "inifiles.html#inifiles_plasmaeffectpara", null ],
        [ "Shape Effect Parameters", "inifiles.html#inifiles_shapeeffectpara", null ],
        [ "Bitmap Effect Paras", "inifiles.html#inifiles_bitmapeffectpara", null ],
        [ "Bitmap Animation Paras", "inifiles.html#inifiles_bitmapanimationpara", null ]
      ] ],
      [ "Setting examples", "inifiles.html#inifiles_settingsexamples", null ],
      [ "LedControl File Testing Application", "inifiles.html#inifiles_testingapp", null ]
    ] ],
    [ "Troubleshooting", "troubleshooting.html", [
      [ "Check B2S.Server Path", "troubleshooting.html#troubleshooting_b2sserver", null ]
    ] ],
    [ "Architecture", "md__d_1_2a_2_direct_output_2_direct_output_2_documentation_250___architecture.html", [
      [ "Object model", "md__d_1_2a_2_direct_output_2_direct_output_2_documentation_250___architecture.html#architecture_objectmodel", [
        [ "Table", "md__d_1_2a_2_direct_output_2_direct_output_2_documentation_250___architecture.html#architecture_table", null ],
        [ "Effects", "md__d_1_2a_2_direct_output_2_direct_output_2_documentation_250___architecture.html#architecture_effects", null ],
        [ "Cabinet", "md__d_1_2a_2_direct_output_2_direct_output_2_documentation_250___architecture.html#architecture_cabinet", [
          [ "Toys", "md__d_1_2a_2_direct_output_2_direct_output_2_documentation_250___architecture.html#architecture_toys", null ],
          [ "Outputcontroller", "md__d_1_2a_2_direct_output_2_direct_output_2_documentation_250___architecture.html#architecture_outputcontroller", null ]
        ] ]
      ] ],
      [ "Multithreading", "md__d_1_2a_2_direct_output_2_direct_output_2_documentation_250___architecture.html#architecture_multithreading", null ]
    ] ],
    [ "B2S Server Plugin", "b2sserverplugin.html", null ],
    [ "Effects/FX", "fx.html", [
      [ "Introduction", "fx.html#fx_introduction", null ],
      [ "Stacking/chaining of effect effects", "fx.html#fx_stacking", null ],
      [ "Built in effects", "fx.html#fx_builtinfx", null ],
      [ "Custom effects", "fx.html#fx_customeffects", null ],
      [ "Implementation guidelines for effects", "fx.html#fx_implementationguideline", null ]
    ] ],
    [ "Built in Effects", "fx_builtin.html", [
      [ "AnalogAlphaMatrixBitmapAnimationEffect", "fx_builtin.html#use_DirectOutput_FX_MatrixFX_AnalogAlphaMatrixBitmapAnimationEffect", [
        [ "Summary", "fx_builtin.html#use_DirectOutput_FX_MatrixFX_AnalogAlphaMatrixBitmapAnimationEffect_summary", null ],
        [ "Sample XML", "fx_builtin.html#use_DirectOutput_FX_MatrixFX_AnalogAlphaMatrixBitmapAnimationEffect_samplexml", null ],
        [ "Properties", "fx_builtin.html#use_DirectOutput_FX_MatrixFX_AnalogAlphaMatrixBitmapAnimationEffect_properties", [
          [ "AnimationBehaviour", "fx_builtin.html#DirectOutput_FX_MatrixFX_AnalogAlphaMatrixBitmapAnimationEffect_AnimationBehaviour", null ],
          [ "AnimationFrameCount", "fx_builtin.html#DirectOutput_FX_MatrixFX_AnalogAlphaMatrixBitmapAnimationEffect_AnimationFrameCount", null ],
          [ "AnimationFrameDurationMs", "fx_builtin.html#DirectOutput_FX_MatrixFX_AnalogAlphaMatrixBitmapAnimationEffect_AnimationFrameDurationMs", null ],
          [ "AnimationStepDirection", "fx_builtin.html#DirectOutput_FX_MatrixFX_AnalogAlphaMatrixBitmapAnimationEffect_AnimationStepDirection", null ],
          [ "AnimationStepSize", "fx_builtin.html#DirectOutput_FX_MatrixFX_AnalogAlphaMatrixBitmapAnimationEffect_AnimationStepSize", null ],
          [ "BitmapFilePattern", "fx_builtin.html#DirectOutput_FX_MatrixFX_AnalogAlphaMatrixBitmapAnimationEffect_BitmapFilePattern", null ],
          [ "BitmapFrameNumber", "fx_builtin.html#DirectOutput_FX_MatrixFX_AnalogAlphaMatrixBitmapAnimationEffect_BitmapFrameNumber", null ],
          [ "BitmapHeight", "fx_builtin.html#DirectOutput_FX_MatrixFX_AnalogAlphaMatrixBitmapAnimationEffect_BitmapHeight", null ],
          [ "BitmapLeft", "fx_builtin.html#DirectOutput_FX_MatrixFX_AnalogAlphaMatrixBitmapAnimationEffect_BitmapLeft", null ],
          [ "BitmapTop", "fx_builtin.html#DirectOutput_FX_MatrixFX_AnalogAlphaMatrixBitmapAnimationEffect_BitmapTop", null ],
          [ "BitmapWidth", "fx_builtin.html#DirectOutput_FX_MatrixFX_AnalogAlphaMatrixBitmapAnimationEffect_BitmapWidth", null ],
          [ "DataExtractMode", "fx_builtin.html#DirectOutput_FX_MatrixFX_AnalogAlphaMatrixBitmapAnimationEffect_DataExtractMode", null ],
          [ "FadeMode", "fx_builtin.html#DirectOutput_FX_MatrixFX_AnalogAlphaMatrixBitmapAnimationEffect_FadeMode", null ],
          [ "Height", "fx_builtin.html#DirectOutput_FX_MatrixFX_AnalogAlphaMatrixBitmapAnimationEffect_Height", null ],
          [ "LayerNr", "fx_builtin.html#DirectOutput_FX_MatrixFX_AnalogAlphaMatrixBitmapAnimationEffect_LayerNr", null ],
          [ "Left", "fx_builtin.html#DirectOutput_FX_MatrixFX_AnalogAlphaMatrixBitmapAnimationEffect_Left", null ],
          [ "Name", "fx_builtin.html#DirectOutput_FX_MatrixFX_AnalogAlphaMatrixBitmapAnimationEffect_Name", null ],
          [ "Top", "fx_builtin.html#DirectOutput_FX_MatrixFX_AnalogAlphaMatrixBitmapAnimationEffect_Top", null ],
          [ "ToyName", "fx_builtin.html#DirectOutput_FX_MatrixFX_AnalogAlphaMatrixBitmapAnimationEffect_ToyName", null ],
          [ "Width", "fx_builtin.html#DirectOutput_FX_MatrixFX_AnalogAlphaMatrixBitmapAnimationEffect_Width", null ]
        ] ]
      ] ],
      [ "AnalogAlphaMatrixBitmapEffect", "fx_builtin.html#use_DirectOutput_FX_MatrixFX_AnalogAlphaMatrixBitmapEffect", [
        [ "Summary", "fx_builtin.html#use_DirectOutput_FX_MatrixFX_AnalogAlphaMatrixBitmapEffect_summary", null ],
        [ "Sample XML", "fx_builtin.html#use_DirectOutput_FX_MatrixFX_AnalogAlphaMatrixBitmapEffect_samplexml", null ],
        [ "Properties", "fx_builtin.html#use_DirectOutput_FX_MatrixFX_AnalogAlphaMatrixBitmapEffect_properties", [
          [ "BitmapFilePattern", "fx_builtin.html#DirectOutput_FX_MatrixFX_AnalogAlphaMatrixBitmapEffect_BitmapFilePattern", null ],
          [ "BitmapFrameNumber", "fx_builtin.html#DirectOutput_FX_MatrixFX_AnalogAlphaMatrixBitmapEffect_BitmapFrameNumber", null ],
          [ "BitmapHeight", "fx_builtin.html#DirectOutput_FX_MatrixFX_AnalogAlphaMatrixBitmapEffect_BitmapHeight", null ],
          [ "BitmapLeft", "fx_builtin.html#DirectOutput_FX_MatrixFX_AnalogAlphaMatrixBitmapEffect_BitmapLeft", null ],
          [ "BitmapTop", "fx_builtin.html#DirectOutput_FX_MatrixFX_AnalogAlphaMatrixBitmapEffect_BitmapTop", null ],
          [ "BitmapWidth", "fx_builtin.html#DirectOutput_FX_MatrixFX_AnalogAlphaMatrixBitmapEffect_BitmapWidth", null ],
          [ "DataExtractMode", "fx_builtin.html#DirectOutput_FX_MatrixFX_AnalogAlphaMatrixBitmapEffect_DataExtractMode", null ],
          [ "FadeMode", "fx_builtin.html#DirectOutput_FX_MatrixFX_AnalogAlphaMatrixBitmapEffect_FadeMode", null ],
          [ "Height", "fx_builtin.html#DirectOutput_FX_MatrixFX_AnalogAlphaMatrixBitmapEffect_Height", null ],
          [ "LayerNr", "fx_builtin.html#DirectOutput_FX_MatrixFX_AnalogAlphaMatrixBitmapEffect_LayerNr", null ],
          [ "Left", "fx_builtin.html#DirectOutput_FX_MatrixFX_AnalogAlphaMatrixBitmapEffect_Left", null ],
          [ "Name", "fx_builtin.html#DirectOutput_FX_MatrixFX_AnalogAlphaMatrixBitmapEffect_Name", null ],
          [ "Top", "fx_builtin.html#DirectOutput_FX_MatrixFX_AnalogAlphaMatrixBitmapEffect_Top", null ],
          [ "ToyName", "fx_builtin.html#DirectOutput_FX_MatrixFX_AnalogAlphaMatrixBitmapEffect_ToyName", null ],
          [ "Width", "fx_builtin.html#DirectOutput_FX_MatrixFX_AnalogAlphaMatrixBitmapEffect_Width", null ]
        ] ]
      ] ],
      [ "AnalogAlphaMatrixFlickerEffect", "fx_builtin.html#use_DirectOutput_FX_MatrixFX_AnalogAlphaMatrixFlickerEffect", [
        [ "Summary", "fx_builtin.html#use_DirectOutput_FX_MatrixFX_AnalogAlphaMatrixFlickerEffect_summary", null ],
        [ "Sample XML", "fx_builtin.html#use_DirectOutput_FX_MatrixFX_AnalogAlphaMatrixFlickerEffect_samplexml", null ],
        [ "Properties", "fx_builtin.html#use_DirectOutput_FX_MatrixFX_AnalogAlphaMatrixFlickerEffect_properties", [
          [ "ActiveValue", "fx_builtin.html#DirectOutput_FX_MatrixFX_AnalogAlphaMatrixFlickerEffect_ActiveValue", null ],
          [ "Density", "fx_builtin.html#DirectOutput_FX_MatrixFX_AnalogAlphaMatrixFlickerEffect_Density", null ],
          [ "FadeMode", "fx_builtin.html#DirectOutput_FX_MatrixFX_AnalogAlphaMatrixFlickerEffect_FadeMode", null ],
          [ "FlickerFadeDownDurationMs", "fx_builtin.html#DirectOutput_FX_MatrixFX_AnalogAlphaMatrixFlickerEffect_FlickerFadeDownDurationMs", null ],
          [ "FlickerFadeUpDurationMs", "fx_builtin.html#DirectOutput_FX_MatrixFX_AnalogAlphaMatrixFlickerEffect_FlickerFadeUpDurationMs", null ],
          [ "Height", "fx_builtin.html#DirectOutput_FX_MatrixFX_AnalogAlphaMatrixFlickerEffect_Height", null ],
          [ "InactiveValue", "fx_builtin.html#DirectOutput_FX_MatrixFX_AnalogAlphaMatrixFlickerEffect_InactiveValue", null ],
          [ "LayerNr", "fx_builtin.html#DirectOutput_FX_MatrixFX_AnalogAlphaMatrixFlickerEffect_LayerNr", null ],
          [ "Left", "fx_builtin.html#DirectOutput_FX_MatrixFX_AnalogAlphaMatrixFlickerEffect_Left", null ],
          [ "MaxFlickerDurationMs", "fx_builtin.html#DirectOutput_FX_MatrixFX_AnalogAlphaMatrixFlickerEffect_MaxFlickerDurationMs", null ],
          [ "MinFlickerDurationMs", "fx_builtin.html#DirectOutput_FX_MatrixFX_AnalogAlphaMatrixFlickerEffect_MinFlickerDurationMs", null ],
          [ "Name", "fx_builtin.html#DirectOutput_FX_MatrixFX_AnalogAlphaMatrixFlickerEffect_Name", null ],
          [ "Top", "fx_builtin.html#DirectOutput_FX_MatrixFX_AnalogAlphaMatrixFlickerEffect_Top", null ],
          [ "ToyName", "fx_builtin.html#DirectOutput_FX_MatrixFX_AnalogAlphaMatrixFlickerEffect_ToyName", null ],
          [ "Width", "fx_builtin.html#DirectOutput_FX_MatrixFX_AnalogAlphaMatrixFlickerEffect_Width", null ]
        ] ]
      ] ],
      [ "AnalogAlphaMatrixShiftEffect", "fx_builtin.html#use_DirectOutput_FX_MatrixFX_AnalogAlphaMatrixShiftEffect", [
        [ "Summary", "fx_builtin.html#use_DirectOutput_FX_MatrixFX_AnalogAlphaMatrixShiftEffect_summary", null ],
        [ "Sample XML", "fx_builtin.html#use_DirectOutput_FX_MatrixFX_AnalogAlphaMatrixShiftEffect_samplexml", null ],
        [ "Properties", "fx_builtin.html#use_DirectOutput_FX_MatrixFX_AnalogAlphaMatrixShiftEffect_properties", [
          [ "ActiveValue", "fx_builtin.html#DirectOutput_FX_MatrixFX_AnalogAlphaMatrixShiftEffect_ActiveValue", null ],
          [ "FadeMode", "fx_builtin.html#DirectOutput_FX_MatrixFX_AnalogAlphaMatrixShiftEffect_FadeMode", null ],
          [ "Height", "fx_builtin.html#DirectOutput_FX_MatrixFX_AnalogAlphaMatrixShiftEffect_Height", null ],
          [ "InactiveValue", "fx_builtin.html#DirectOutput_FX_MatrixFX_AnalogAlphaMatrixShiftEffect_InactiveValue", null ],
          [ "LayerNr", "fx_builtin.html#DirectOutput_FX_MatrixFX_AnalogAlphaMatrixShiftEffect_LayerNr", null ],
          [ "Left", "fx_builtin.html#DirectOutput_FX_MatrixFX_AnalogAlphaMatrixShiftEffect_Left", null ],
          [ "Name", "fx_builtin.html#DirectOutput_FX_MatrixFX_AnalogAlphaMatrixShiftEffect_Name", null ],
          [ "ShiftAcceleration", "fx_builtin.html#DirectOutput_FX_MatrixFX_AnalogAlphaMatrixShiftEffect_ShiftAcceleration", null ],
          [ "ShiftDirection", "fx_builtin.html#DirectOutput_FX_MatrixFX_AnalogAlphaMatrixShiftEffect_ShiftDirection", null ],
          [ "ShiftSpeed", "fx_builtin.html#DirectOutput_FX_MatrixFX_AnalogAlphaMatrixShiftEffect_ShiftSpeed", null ],
          [ "Top", "fx_builtin.html#DirectOutput_FX_MatrixFX_AnalogAlphaMatrixShiftEffect_Top", null ],
          [ "ToyName", "fx_builtin.html#DirectOutput_FX_MatrixFX_AnalogAlphaMatrixShiftEffect_ToyName", null ],
          [ "Width", "fx_builtin.html#DirectOutput_FX_MatrixFX_AnalogAlphaMatrixShiftEffect_Width", null ]
        ] ]
      ] ],
      [ "AnalogAlphaMatrixValueEffect", "fx_builtin.html#use_DirectOutput_FX_MatrixFX_AnalogAlphaMatrixValueEffect", [
        [ "Summary", "fx_builtin.html#use_DirectOutput_FX_MatrixFX_AnalogAlphaMatrixValueEffect_summary", null ],
        [ "Sample XML", "fx_builtin.html#use_DirectOutput_FX_MatrixFX_AnalogAlphaMatrixValueEffect_samplexml", null ],
        [ "Properties", "fx_builtin.html#use_DirectOutput_FX_MatrixFX_AnalogAlphaMatrixValueEffect_properties", [
          [ "ActiveValue", "fx_builtin.html#DirectOutput_FX_MatrixFX_AnalogAlphaMatrixValueEffect_ActiveValue", null ],
          [ "FadeMode", "fx_builtin.html#DirectOutput_FX_MatrixFX_AnalogAlphaMatrixValueEffect_FadeMode", null ],
          [ "Height", "fx_builtin.html#DirectOutput_FX_MatrixFX_AnalogAlphaMatrixValueEffect_Height", null ],
          [ "InactiveValue", "fx_builtin.html#DirectOutput_FX_MatrixFX_AnalogAlphaMatrixValueEffect_InactiveValue", null ],
          [ "LayerNr", "fx_builtin.html#DirectOutput_FX_MatrixFX_AnalogAlphaMatrixValueEffect_LayerNr", null ],
          [ "Left", "fx_builtin.html#DirectOutput_FX_MatrixFX_AnalogAlphaMatrixValueEffect_Left", null ],
          [ "Name", "fx_builtin.html#DirectOutput_FX_MatrixFX_AnalogAlphaMatrixValueEffect_Name", null ],
          [ "Top", "fx_builtin.html#DirectOutput_FX_MatrixFX_AnalogAlphaMatrixValueEffect_Top", null ],
          [ "ToyName", "fx_builtin.html#DirectOutput_FX_MatrixFX_AnalogAlphaMatrixValueEffect_ToyName", null ],
          [ "Width", "fx_builtin.html#DirectOutput_FX_MatrixFX_AnalogAlphaMatrixValueEffect_Width", null ]
        ] ]
      ] ],
      [ "AnalogToyValueEffect", "fx_builtin.html#use_DirectOutput_FX_AnalogToyFX_AnalogToyValueEffect", [
        [ "Summary", "fx_builtin.html#use_DirectOutput_FX_AnalogToyFX_AnalogToyValueEffect_summary", null ],
        [ "Sample XML", "fx_builtin.html#use_DirectOutput_FX_AnalogToyFX_AnalogToyValueEffect_samplexml", null ],
        [ "Properties", "fx_builtin.html#use_DirectOutput_FX_AnalogToyFX_AnalogToyValueEffect_properties", [
          [ "ActiveValue", "fx_builtin.html#DirectOutput_FX_AnalogToyFX_AnalogToyValueEffect_ActiveValue", null ],
          [ "FadeMode", "fx_builtin.html#DirectOutput_FX_AnalogToyFX_AnalogToyValueEffect_FadeMode", null ],
          [ "InactiveValue", "fx_builtin.html#DirectOutput_FX_AnalogToyFX_AnalogToyValueEffect_InactiveValue", null ],
          [ "LayerNr", "fx_builtin.html#DirectOutput_FX_AnalogToyFX_AnalogToyValueEffect_LayerNr", null ],
          [ "Name", "fx_builtin.html#DirectOutput_FX_AnalogToyFX_AnalogToyValueEffect_Name", null ],
          [ "ToyName", "fx_builtin.html#DirectOutput_FX_AnalogToyFX_AnalogToyValueEffect_ToyName", null ]
        ] ]
      ] ],
      [ "BlinkEffect", "fx_builtin.html#use_DirectOutput_FX_TimmedFX_BlinkEffect", [
        [ "Summary", "fx_builtin.html#use_DirectOutput_FX_TimmedFX_BlinkEffect_summary", null ],
        [ "Sample XML", "fx_builtin.html#use_DirectOutput_FX_TimmedFX_BlinkEffect_samplexml", null ],
        [ "Properties", "fx_builtin.html#use_DirectOutput_FX_TimmedFX_BlinkEffect_properties", [
          [ "DurationActiveMs", "fx_builtin.html#DirectOutput_FX_TimmedFX_BlinkEffect_DurationActiveMs", null ],
          [ "DurationInactiveMs", "fx_builtin.html#DirectOutput_FX_TimmedFX_BlinkEffect_DurationInactiveMs", null ],
          [ "HighValue", "fx_builtin.html#DirectOutput_FX_TimmedFX_BlinkEffect_HighValue", null ],
          [ "LowValue", "fx_builtin.html#DirectOutput_FX_TimmedFX_BlinkEffect_LowValue", null ],
          [ "Name", "fx_builtin.html#DirectOutput_FX_TimmedFX_BlinkEffect_Name", null ],
          [ "TargetEffectName", "fx_builtin.html#DirectOutput_FX_TimmedFX_BlinkEffect_TargetEffectName", null ],
          [ "UntriggerBehaviour", "fx_builtin.html#DirectOutput_FX_TimmedFX_BlinkEffect_UntriggerBehaviour", null ]
        ] ]
      ] ],
      [ "DelayEffect", "fx_builtin.html#use_DirectOutput_FX_TimmedFX_DelayEffect", [
        [ "Summary", "fx_builtin.html#use_DirectOutput_FX_TimmedFX_DelayEffect_summary", null ],
        [ "Sample XML", "fx_builtin.html#use_DirectOutput_FX_TimmedFX_DelayEffect_samplexml", null ],
        [ "Properties", "fx_builtin.html#use_DirectOutput_FX_TimmedFX_DelayEffect_properties", [
          [ "DelayMs", "fx_builtin.html#DirectOutput_FX_TimmedFX_DelayEffect_DelayMs", null ],
          [ "Name", "fx_builtin.html#DirectOutput_FX_TimmedFX_DelayEffect_Name", null ],
          [ "TargetEffectName", "fx_builtin.html#DirectOutput_FX_TimmedFX_DelayEffect_TargetEffectName", null ]
        ] ]
      ] ],
      [ "DurationEffect", "fx_builtin.html#use_DirectOutput_FX_TimmedFX_DurationEffect", [
        [ "Summary", "fx_builtin.html#use_DirectOutput_FX_TimmedFX_DurationEffect_summary", null ],
        [ "Sample XML", "fx_builtin.html#use_DirectOutput_FX_TimmedFX_DurationEffect_samplexml", null ],
        [ "Properties", "fx_builtin.html#use_DirectOutput_FX_TimmedFX_DurationEffect_properties", [
          [ "DurationMs", "fx_builtin.html#DirectOutput_FX_TimmedFX_DurationEffect_DurationMs", null ],
          [ "Name", "fx_builtin.html#DirectOutput_FX_TimmedFX_DurationEffect_Name", null ],
          [ "RetriggerBehaviour", "fx_builtin.html#DirectOutput_FX_TimmedFX_DurationEffect_RetriggerBehaviour", null ],
          [ "TargetEffectName", "fx_builtin.html#DirectOutput_FX_TimmedFX_DurationEffect_TargetEffectName", null ]
        ] ]
      ] ],
      [ "ExtendDurationEffect", "fx_builtin.html#use_DirectOutput_FX_TimmedFX_ExtendDurationEffect", [
        [ "Summary", "fx_builtin.html#use_DirectOutput_FX_TimmedFX_ExtendDurationEffect_summary", null ],
        [ "Sample XML", "fx_builtin.html#use_DirectOutput_FX_TimmedFX_ExtendDurationEffect_samplexml", null ],
        [ "Properties", "fx_builtin.html#use_DirectOutput_FX_TimmedFX_ExtendDurationEffect_properties", [
          [ "DurationMs", "fx_builtin.html#DirectOutput_FX_TimmedFX_ExtendDurationEffect_DurationMs", null ],
          [ "Name", "fx_builtin.html#DirectOutput_FX_TimmedFX_ExtendDurationEffect_Name", null ],
          [ "TargetEffectName", "fx_builtin.html#DirectOutput_FX_TimmedFX_ExtendDurationEffect_TargetEffectName", null ]
        ] ]
      ] ],
      [ "FadeEffect", "fx_builtin.html#use_DirectOutput_FX_TimmedFX_FadeEffect", [
        [ "Summary", "fx_builtin.html#use_DirectOutput_FX_TimmedFX_FadeEffect_summary", null ],
        [ "Sample XML", "fx_builtin.html#use_DirectOutput_FX_TimmedFX_FadeEffect_samplexml", null ],
        [ "Properties", "fx_builtin.html#use_DirectOutput_FX_TimmedFX_FadeEffect_properties", [
          [ "FadeDownDuration", "fx_builtin.html#DirectOutput_FX_TimmedFX_FadeEffect_FadeDownDuration", null ],
          [ "FadeDurationMode", "fx_builtin.html#DirectOutput_FX_TimmedFX_FadeEffect_FadeDurationMode", null ],
          [ "FadeUpDuration", "fx_builtin.html#DirectOutput_FX_TimmedFX_FadeEffect_FadeUpDuration", null ],
          [ "Name", "fx_builtin.html#DirectOutput_FX_TimmedFX_FadeEffect_Name", null ],
          [ "TargetEffectName", "fx_builtin.html#DirectOutput_FX_TimmedFX_FadeEffect_TargetEffectName", null ]
        ] ]
      ] ],
      [ "ListEffect", "fx_builtin.html#use_DirectOutput_FX_ListFX_ListEffect", [
        [ "Summary", "fx_builtin.html#use_DirectOutput_FX_ListFX_ListEffect_summary", null ],
        [ "Sample XML", "fx_builtin.html#use_DirectOutput_FX_ListFX_ListEffect_samplexml", null ],
        [ "Properties", "fx_builtin.html#use_DirectOutput_FX_ListFX_ListEffect_properties", [
          [ "AssignedEffects", "fx_builtin.html#DirectOutput_FX_ListFX_ListEffect_AssignedEffects", null ],
          [ "Name", "fx_builtin.html#DirectOutput_FX_ListFX_ListEffect_Name", null ]
        ] ]
      ] ],
      [ "MaxDurationEffect", "fx_builtin.html#use_DirectOutput_FX_TimmedFX_MaxDurationEffect", [
        [ "Summary", "fx_builtin.html#use_DirectOutput_FX_TimmedFX_MaxDurationEffect_summary", null ],
        [ "Sample XML", "fx_builtin.html#use_DirectOutput_FX_TimmedFX_MaxDurationEffect_samplexml", null ],
        [ "Properties", "fx_builtin.html#use_DirectOutput_FX_TimmedFX_MaxDurationEffect_properties", [
          [ "MaxDurationMs", "fx_builtin.html#DirectOutput_FX_TimmedFX_MaxDurationEffect_MaxDurationMs", null ],
          [ "Name", "fx_builtin.html#DirectOutput_FX_TimmedFX_MaxDurationEffect_Name", null ],
          [ "RetriggerBehaviour", "fx_builtin.html#DirectOutput_FX_TimmedFX_MaxDurationEffect_RetriggerBehaviour", null ],
          [ "TargetEffectName", "fx_builtin.html#DirectOutput_FX_TimmedFX_MaxDurationEffect_TargetEffectName", null ]
        ] ]
      ] ],
      [ "MinDurationEffect", "fx_builtin.html#use_DirectOutput_FX_TimmedFX_MinDurationEffect", [
        [ "Summary", "fx_builtin.html#use_DirectOutput_FX_TimmedFX_MinDurationEffect_summary", null ],
        [ "Sample XML", "fx_builtin.html#use_DirectOutput_FX_TimmedFX_MinDurationEffect_samplexml", null ],
        [ "Properties", "fx_builtin.html#use_DirectOutput_FX_TimmedFX_MinDurationEffect_properties", [
          [ "MinDurationMs", "fx_builtin.html#DirectOutput_FX_TimmedFX_MinDurationEffect_MinDurationMs", null ],
          [ "Name", "fx_builtin.html#DirectOutput_FX_TimmedFX_MinDurationEffect_Name", null ],
          [ "RetriggerBehaviour", "fx_builtin.html#DirectOutput_FX_TimmedFX_MinDurationEffect_RetriggerBehaviour", null ],
          [ "TargetEffectName", "fx_builtin.html#DirectOutput_FX_TimmedFX_MinDurationEffect_TargetEffectName", null ]
        ] ]
      ] ],
      [ "NullEffect", "fx_builtin.html#use_DirectOutput_FX_NullFX_NullEffect", [
        [ "Summary", "fx_builtin.html#use_DirectOutput_FX_NullFX_NullEffect_summary", null ],
        [ "Sample XML", "fx_builtin.html#use_DirectOutput_FX_NullFX_NullEffect_samplexml", null ],
        [ "Properties", "fx_builtin.html#use_DirectOutput_FX_NullFX_NullEffect_properties", [
          [ "Name", "fx_builtin.html#DirectOutput_FX_NullFX_NullEffect_Name", null ]
        ] ]
      ] ],
      [ "RGBAColorEffect", "fx_builtin.html#use_DirectOutput_FX_RGBAFX_RGBAColorEffect", [
        [ "Summary", "fx_builtin.html#use_DirectOutput_FX_RGBAFX_RGBAColorEffect_summary", null ],
        [ "Sample XML", "fx_builtin.html#use_DirectOutput_FX_RGBAFX_RGBAColorEffect_samplexml", null ],
        [ "Properties", "fx_builtin.html#use_DirectOutput_FX_RGBAFX_RGBAColorEffect_properties", [
          [ "ActiveColor", "fx_builtin.html#DirectOutput_FX_RGBAFX_RGBAColorEffect_ActiveColor", null ],
          [ "FadeMode", "fx_builtin.html#DirectOutput_FX_RGBAFX_RGBAColorEffect_FadeMode", null ],
          [ "InactiveColor", "fx_builtin.html#DirectOutput_FX_RGBAFX_RGBAColorEffect_InactiveColor", null ],
          [ "LayerNr", "fx_builtin.html#DirectOutput_FX_RGBAFX_RGBAColorEffect_LayerNr", null ],
          [ "Name", "fx_builtin.html#DirectOutput_FX_RGBAFX_RGBAColorEffect_Name", null ],
          [ "ToyName", "fx_builtin.html#DirectOutput_FX_RGBAFX_RGBAColorEffect_ToyName", null ]
        ] ]
      ] ],
      [ "RGBAMatrixBitmapAnimationEffect", "fx_builtin.html#use_DirectOutput_FX_MatrixFX_RGBAMatrixBitmapAnimationEffect", [
        [ "Summary", "fx_builtin.html#use_DirectOutput_FX_MatrixFX_RGBAMatrixBitmapAnimationEffect_summary", null ],
        [ "Sample XML", "fx_builtin.html#use_DirectOutput_FX_MatrixFX_RGBAMatrixBitmapAnimationEffect_samplexml", null ],
        [ "Properties", "fx_builtin.html#use_DirectOutput_FX_MatrixFX_RGBAMatrixBitmapAnimationEffect_properties", [
          [ "AnimationBehaviour", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixBitmapAnimationEffect_AnimationBehaviour", null ],
          [ "AnimationFrameCount", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixBitmapAnimationEffect_AnimationFrameCount", null ],
          [ "AnimationFrameDurationMs", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixBitmapAnimationEffect_AnimationFrameDurationMs", null ],
          [ "AnimationStepDirection", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixBitmapAnimationEffect_AnimationStepDirection", null ],
          [ "AnimationStepSize", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixBitmapAnimationEffect_AnimationStepSize", null ],
          [ "BitmapFilePattern", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixBitmapAnimationEffect_BitmapFilePattern", null ],
          [ "BitmapFrameNumber", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixBitmapAnimationEffect_BitmapFrameNumber", null ],
          [ "BitmapHeight", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixBitmapAnimationEffect_BitmapHeight", null ],
          [ "BitmapLeft", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixBitmapAnimationEffect_BitmapLeft", null ],
          [ "BitmapTop", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixBitmapAnimationEffect_BitmapTop", null ],
          [ "BitmapWidth", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixBitmapAnimationEffect_BitmapWidth", null ],
          [ "DataExtractMode", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixBitmapAnimationEffect_DataExtractMode", null ],
          [ "FadeMode", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixBitmapAnimationEffect_FadeMode", null ],
          [ "Height", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixBitmapAnimationEffect_Height", null ],
          [ "LayerNr", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixBitmapAnimationEffect_LayerNr", null ],
          [ "Left", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixBitmapAnimationEffect_Left", null ],
          [ "Name", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixBitmapAnimationEffect_Name", null ],
          [ "Top", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixBitmapAnimationEffect_Top", null ],
          [ "ToyName", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixBitmapAnimationEffect_ToyName", null ],
          [ "Width", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixBitmapAnimationEffect_Width", null ]
        ] ]
      ] ],
      [ "RGBAMatrixBitmapEffect", "fx_builtin.html#use_DirectOutput_FX_MatrixFX_RGBAMatrixBitmapEffect", [
        [ "Summary", "fx_builtin.html#use_DirectOutput_FX_MatrixFX_RGBAMatrixBitmapEffect_summary", null ],
        [ "Sample XML", "fx_builtin.html#use_DirectOutput_FX_MatrixFX_RGBAMatrixBitmapEffect_samplexml", null ],
        [ "Properties", "fx_builtin.html#use_DirectOutput_FX_MatrixFX_RGBAMatrixBitmapEffect_properties", [
          [ "BitmapFilePattern", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixBitmapEffect_BitmapFilePattern", null ],
          [ "BitmapFrameNumber", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixBitmapEffect_BitmapFrameNumber", null ],
          [ "BitmapHeight", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixBitmapEffect_BitmapHeight", null ],
          [ "BitmapLeft", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixBitmapEffect_BitmapLeft", null ],
          [ "BitmapTop", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixBitmapEffect_BitmapTop", null ],
          [ "BitmapWidth", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixBitmapEffect_BitmapWidth", null ],
          [ "DataExtractMode", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixBitmapEffect_DataExtractMode", null ],
          [ "FadeMode", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixBitmapEffect_FadeMode", null ],
          [ "Height", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixBitmapEffect_Height", null ],
          [ "LayerNr", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixBitmapEffect_LayerNr", null ],
          [ "Left", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixBitmapEffect_Left", null ],
          [ "Name", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixBitmapEffect_Name", null ],
          [ "Top", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixBitmapEffect_Top", null ],
          [ "ToyName", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixBitmapEffect_ToyName", null ],
          [ "Width", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixBitmapEffect_Width", null ]
        ] ]
      ] ],
      [ "RGBAMatrixColorEffect", "fx_builtin.html#use_DirectOutput_FX_MatrixFX_RGBAMatrixColorEffect", [
        [ "Summary", "fx_builtin.html#use_DirectOutput_FX_MatrixFX_RGBAMatrixColorEffect_summary", null ],
        [ "Sample XML", "fx_builtin.html#use_DirectOutput_FX_MatrixFX_RGBAMatrixColorEffect_samplexml", null ],
        [ "Properties", "fx_builtin.html#use_DirectOutput_FX_MatrixFX_RGBAMatrixColorEffect_properties", [
          [ "ActiveColor", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixColorEffect_ActiveColor", null ],
          [ "FadeMode", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixColorEffect_FadeMode", null ],
          [ "Height", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixColorEffect_Height", null ],
          [ "InactiveColor", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixColorEffect_InactiveColor", null ],
          [ "LayerNr", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixColorEffect_LayerNr", null ],
          [ "Left", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixColorEffect_Left", null ],
          [ "Name", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixColorEffect_Name", null ],
          [ "Top", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixColorEffect_Top", null ],
          [ "ToyName", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixColorEffect_ToyName", null ],
          [ "Width", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixColorEffect_Width", null ]
        ] ]
      ] ],
      [ "RGBAMatrixColorScaleBitmapAnimationEffect", "fx_builtin.html#use_DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapAnimationEffect", [
        [ "Summary", "fx_builtin.html#use_DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapAnimationEffect_summary", null ],
        [ "Sample XML", "fx_builtin.html#use_DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapAnimationEffect_samplexml", null ],
        [ "Properties", "fx_builtin.html#use_DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapAnimationEffect_properties", [
          [ "ActiveColor", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapAnimationEffect_ActiveColor", null ],
          [ "AnimationBehaviour", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapAnimationEffect_AnimationBehaviour", null ],
          [ "AnimationFrameCount", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapAnimationEffect_AnimationFrameCount", null ],
          [ "AnimationFrameDurationMs", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapAnimationEffect_AnimationFrameDurationMs", null ],
          [ "AnimationStepDirection", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapAnimationEffect_AnimationStepDirection", null ],
          [ "AnimationStepSize", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapAnimationEffect_AnimationStepSize", null ],
          [ "BitmapFilePattern", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapAnimationEffect_BitmapFilePattern", null ],
          [ "BitmapFrameNumber", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapAnimationEffect_BitmapFrameNumber", null ],
          [ "BitmapHeight", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapAnimationEffect_BitmapHeight", null ],
          [ "BitmapLeft", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapAnimationEffect_BitmapLeft", null ],
          [ "BitmapTop", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapAnimationEffect_BitmapTop", null ],
          [ "BitmapWidth", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapAnimationEffect_BitmapWidth", null ],
          [ "DataExtractMode", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapAnimationEffect_DataExtractMode", null ],
          [ "FadeMode", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapAnimationEffect_FadeMode", null ],
          [ "Height", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapAnimationEffect_Height", null ],
          [ "InactiveColor", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapAnimationEffect_InactiveColor", null ],
          [ "LayerNr", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapAnimationEffect_LayerNr", null ],
          [ "Left", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapAnimationEffect_Left", null ],
          [ "Name", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapAnimationEffect_Name", null ],
          [ "Top", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapAnimationEffect_Top", null ],
          [ "ToyName", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapAnimationEffect_ToyName", null ],
          [ "Width", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapAnimationEffect_Width", null ]
        ] ]
      ] ],
      [ "RGBAMatrixColorScaleBitmapEffect", "fx_builtin.html#use_DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapEffect", [
        [ "Summary", "fx_builtin.html#use_DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapEffect_summary", null ],
        [ "Sample XML", "fx_builtin.html#use_DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapEffect_samplexml", null ],
        [ "Properties", "fx_builtin.html#use_DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapEffect_properties", [
          [ "ActiveColor", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapEffect_ActiveColor", null ],
          [ "BitmapFilePattern", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapEffect_BitmapFilePattern", null ],
          [ "BitmapFrameNumber", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapEffect_BitmapFrameNumber", null ],
          [ "BitmapHeight", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapEffect_BitmapHeight", null ],
          [ "BitmapLeft", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapEffect_BitmapLeft", null ],
          [ "BitmapTop", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapEffect_BitmapTop", null ],
          [ "BitmapWidth", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapEffect_BitmapWidth", null ],
          [ "DataExtractMode", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapEffect_DataExtractMode", null ],
          [ "FadeMode", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapEffect_FadeMode", null ],
          [ "Height", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapEffect_Height", null ],
          [ "InactiveColor", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapEffect_InactiveColor", null ],
          [ "LayerNr", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapEffect_LayerNr", null ],
          [ "Left", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapEffect_Left", null ],
          [ "Name", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapEffect_Name", null ],
          [ "Top", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapEffect_Top", null ],
          [ "ToyName", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapEffect_ToyName", null ],
          [ "Width", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleBitmapEffect_Width", null ]
        ] ]
      ] ],
      [ "RGBAMatrixColorScaleShapeEffect", "fx_builtin.html#use_DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleShapeEffect", [
        [ "Summary", "fx_builtin.html#use_DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleShapeEffect_summary", null ],
        [ "Sample XML", "fx_builtin.html#use_DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleShapeEffect_samplexml", null ],
        [ "Properties", "fx_builtin.html#use_DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleShapeEffect_properties", [
          [ "ActiveColor", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleShapeEffect_ActiveColor", null ],
          [ "FadeMode", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleShapeEffect_FadeMode", null ],
          [ "Height", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleShapeEffect_Height", null ],
          [ "InactiveColor", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleShapeEffect_InactiveColor", null ],
          [ "LayerNr", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleShapeEffect_LayerNr", null ],
          [ "Left", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleShapeEffect_Left", null ],
          [ "Name", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleShapeEffect_Name", null ],
          [ "ShapeName", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleShapeEffect_ShapeName", null ],
          [ "Top", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleShapeEffect_Top", null ],
          [ "ToyName", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleShapeEffect_ToyName", null ],
          [ "Width", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixColorScaleShapeEffect_Width", null ]
        ] ]
      ] ],
      [ "RGBAMatrixFlickerEffect", "fx_builtin.html#use_DirectOutput_FX_MatrixFX_RGBAMatrixFlickerEffect", [
        [ "Summary", "fx_builtin.html#use_DirectOutput_FX_MatrixFX_RGBAMatrixFlickerEffect_summary", null ],
        [ "Sample XML", "fx_builtin.html#use_DirectOutput_FX_MatrixFX_RGBAMatrixFlickerEffect_samplexml", null ],
        [ "Properties", "fx_builtin.html#use_DirectOutput_FX_MatrixFX_RGBAMatrixFlickerEffect_properties", [
          [ "ActiveColor", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixFlickerEffect_ActiveColor", null ],
          [ "Density", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixFlickerEffect_Density", null ],
          [ "FadeMode", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixFlickerEffect_FadeMode", null ],
          [ "FlickerFadeDownDurationMs", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixFlickerEffect_FlickerFadeDownDurationMs", null ],
          [ "FlickerFadeUpDurationMs", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixFlickerEffect_FlickerFadeUpDurationMs", null ],
          [ "Height", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixFlickerEffect_Height", null ],
          [ "InactiveColor", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixFlickerEffect_InactiveColor", null ],
          [ "LayerNr", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixFlickerEffect_LayerNr", null ],
          [ "Left", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixFlickerEffect_Left", null ],
          [ "MaxFlickerDurationMs", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixFlickerEffect_MaxFlickerDurationMs", null ],
          [ "MinFlickerDurationMs", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixFlickerEffect_MinFlickerDurationMs", null ],
          [ "Name", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixFlickerEffect_Name", null ],
          [ "Top", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixFlickerEffect_Top", null ],
          [ "ToyName", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixFlickerEffect_ToyName", null ],
          [ "Width", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixFlickerEffect_Width", null ]
        ] ]
      ] ],
      [ "RGBAMatrixPlasmaEffect", "fx_builtin.html#use_DirectOutput_FX_MatrixFX_RGBAMatrixPlasmaEffect", [
        [ "Summary", "fx_builtin.html#use_DirectOutput_FX_MatrixFX_RGBAMatrixPlasmaEffect_summary", null ],
        [ "Sample XML", "fx_builtin.html#use_DirectOutput_FX_MatrixFX_RGBAMatrixPlasmaEffect_samplexml", null ],
        [ "Properties", "fx_builtin.html#use_DirectOutput_FX_MatrixFX_RGBAMatrixPlasmaEffect_properties", [
          [ "ActiveColor1", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixPlasmaEffect_ActiveColor1", null ],
          [ "ActiveColor2", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixPlasmaEffect_ActiveColor2", null ],
          [ "FadeMode", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixPlasmaEffect_FadeMode", null ],
          [ "Height", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixPlasmaEffect_Height", null ],
          [ "InactiveColor", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixPlasmaEffect_InactiveColor", null ],
          [ "LayerNr", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixPlasmaEffect_LayerNr", null ],
          [ "Left", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixPlasmaEffect_Left", null ],
          [ "Name", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixPlasmaEffect_Name", null ],
          [ "PlasmaDensity", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixPlasmaEffect_PlasmaDensity", null ],
          [ "PlasmaScale", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixPlasmaEffect_PlasmaScale", null ],
          [ "PlasmaSpeed", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixPlasmaEffect_PlasmaSpeed", null ],
          [ "Top", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixPlasmaEffect_Top", null ],
          [ "ToyName", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixPlasmaEffect_ToyName", null ],
          [ "Width", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixPlasmaEffect_Width", null ]
        ] ]
      ] ],
      [ "RGBAMatrixShapeEffect", "fx_builtin.html#use_DirectOutput_FX_MatrixFX_RGBAMatrixShapeEffect", [
        [ "Summary", "fx_builtin.html#use_DirectOutput_FX_MatrixFX_RGBAMatrixShapeEffect_summary", null ],
        [ "Sample XML", "fx_builtin.html#use_DirectOutput_FX_MatrixFX_RGBAMatrixShapeEffect_samplexml", null ],
        [ "Properties", "fx_builtin.html#use_DirectOutput_FX_MatrixFX_RGBAMatrixShapeEffect_properties", [
          [ "FadeMode", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixShapeEffect_FadeMode", null ],
          [ "Height", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixShapeEffect_Height", null ],
          [ "LayerNr", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixShapeEffect_LayerNr", null ],
          [ "Left", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixShapeEffect_Left", null ],
          [ "Name", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixShapeEffect_Name", null ],
          [ "ShapeName", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixShapeEffect_ShapeName", null ],
          [ "Top", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixShapeEffect_Top", null ],
          [ "ToyName", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixShapeEffect_ToyName", null ],
          [ "Width", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixShapeEffect_Width", null ]
        ] ]
      ] ],
      [ "RGBAMatrixShiftEffect", "fx_builtin.html#use_DirectOutput_FX_MatrixFX_RGBAMatrixShiftEffect", [
        [ "Sample XML", "fx_builtin.html#use_DirectOutput_FX_MatrixFX_RGBAMatrixShiftEffect_samplexml", null ],
        [ "Properties", "fx_builtin.html#use_DirectOutput_FX_MatrixFX_RGBAMatrixShiftEffect_properties", [
          [ "ActiveColor", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixShiftEffect_ActiveColor", null ],
          [ "FadeMode", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixShiftEffect_FadeMode", null ],
          [ "Height", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixShiftEffect_Height", null ],
          [ "InactiveColor", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixShiftEffect_InactiveColor", null ],
          [ "LayerNr", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixShiftEffect_LayerNr", null ],
          [ "Left", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixShiftEffect_Left", null ],
          [ "Name", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixShiftEffect_Name", null ],
          [ "ShiftAcceleration", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixShiftEffect_ShiftAcceleration", null ],
          [ "ShiftDirection", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixShiftEffect_ShiftDirection", null ],
          [ "ShiftSpeed", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixShiftEffect_ShiftSpeed", null ],
          [ "Top", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixShiftEffect_Top", null ],
          [ "ToyName", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixShiftEffect_ToyName", null ],
          [ "Width", "fx_builtin.html#DirectOutput_FX_MatrixFX_RGBAMatrixShiftEffect_Width", null ]
        ] ]
      ] ],
      [ "TableElementConditionEffect", "fx_builtin.html#use_DirectOutput_FX_ConditionFX_TableElementConditionEffect", [
        [ "Summary", "fx_builtin.html#use_DirectOutput_FX_ConditionFX_TableElementConditionEffect_summary", null ],
        [ "Sample XML", "fx_builtin.html#use_DirectOutput_FX_ConditionFX_TableElementConditionEffect_samplexml", null ],
        [ "Properties", "fx_builtin.html#use_DirectOutput_FX_ConditionFX_TableElementConditionEffect_properties", [
          [ "Condition", "fx_builtin.html#DirectOutput_FX_ConditionFX_TableElementConditionEffect_Condition", null ],
          [ "Name", "fx_builtin.html#DirectOutput_FX_ConditionFX_TableElementConditionEffect_Name", null ],
          [ "TargetEffectName", "fx_builtin.html#DirectOutput_FX_ConditionFX_TableElementConditionEffect_TargetEffectName", null ]
        ] ]
      ] ],
      [ "ValueInvertEffect", "fx_builtin.html#use_DirectOutput_FX_ValueFX_ValueInvertEffect", [
        [ "Summary", "fx_builtin.html#use_DirectOutput_FX_ValueFX_ValueInvertEffect_summary", null ],
        [ "Sample XML", "fx_builtin.html#use_DirectOutput_FX_ValueFX_ValueInvertEffect_samplexml", null ],
        [ "Properties", "fx_builtin.html#use_DirectOutput_FX_ValueFX_ValueInvertEffect_properties", [
          [ "Name", "fx_builtin.html#DirectOutput_FX_ValueFX_ValueInvertEffect_Name", null ],
          [ "TargetEffectName", "fx_builtin.html#DirectOutput_FX_ValueFX_ValueInvertEffect_TargetEffectName", null ]
        ] ]
      ] ],
      [ "ValueMapFullRangeEffect", "fx_builtin.html#use_DirectOutput_FX_ValueFX_ValueMapFullRangeEffect", [
        [ "Summary", "fx_builtin.html#use_DirectOutput_FX_ValueFX_ValueMapFullRangeEffect_summary", null ],
        [ "Sample XML", "fx_builtin.html#use_DirectOutput_FX_ValueFX_ValueMapFullRangeEffect_samplexml", null ],
        [ "Properties", "fx_builtin.html#use_DirectOutput_FX_ValueFX_ValueMapFullRangeEffect_properties", [
          [ "Name", "fx_builtin.html#DirectOutput_FX_ValueFX_ValueMapFullRangeEffect_Name", null ],
          [ "TargetEffectName", "fx_builtin.html#DirectOutput_FX_ValueFX_ValueMapFullRangeEffect_TargetEffectName", null ]
        ] ]
      ] ]
    ] ],
    [ "Toys", "toys.html", [
      [ "Introduction", "toys.html#toys_introduction", null ],
      [ "Builtin toys", "toys.html#toys_bultin", null ],
      [ "Implementation guidelines for custom toys", "toys.html#toys_implementationguideline", null ]
    ] ],
    [ "Built in Toys", "toy_builtin.html", [
      [ "AnalogAlphaToy", "toy_builtin.html#use_DirectOutput_Cab_Toys_Layer_AnalogAlphaToy", [
        [ "Summary", "toy_builtin.html#use_DirectOutput_Cab_Toys_Layer_AnalogAlphaToy_summary", null ],
        [ "Sample XML", "toy_builtin.html#use_DirectOutput_Cab_Toys_Layer_AnalogAlphaToy_samplexml", null ],
        [ "Properties", "toy_builtin.html#use_DirectOutput_Cab_Toys_Layer_AnalogAlphaToy_properties", [
          [ "FadingCurveName", "toy_builtin.html#DirectOutput_Cab_Toys_Layer_AnalogAlphaToy_FadingCurveName", null ],
          [ "Name", "toy_builtin.html#DirectOutput_Cab_Toys_Layer_AnalogAlphaToy_Name", null ],
          [ "OutputName", "toy_builtin.html#DirectOutput_Cab_Toys_Layer_AnalogAlphaToy_OutputName", null ]
        ] ]
      ] ],
      [ "AnalogAlphaToyGroup", "toy_builtin.html#use_DirectOutput_Cab_Toys_Virtual_AnalogAlphaToyGroup", [
        [ "Summary", "toy_builtin.html#use_DirectOutput_Cab_Toys_Virtual_AnalogAlphaToyGroup_summary", null ],
        [ "Sample XML", "toy_builtin.html#use_DirectOutput_Cab_Toys_Virtual_AnalogAlphaToyGroup_samplexml", null ],
        [ "Properties", "toy_builtin.html#use_DirectOutput_Cab_Toys_Virtual_AnalogAlphaToyGroup_properties", [
          [ "LayerOffset", "toy_builtin.html#DirectOutput_Cab_Toys_Virtual_AnalogAlphaToyGroup_LayerOffset", null ],
          [ "Name", "toy_builtin.html#DirectOutput_Cab_Toys_Virtual_AnalogAlphaToyGroup_Name", null ],
          [ "ToyNames", "toy_builtin.html#DirectOutput_Cab_Toys_Virtual_AnalogAlphaToyGroup_ToyNames", null ]
        ] ]
      ] ],
      [ "GearMotor", "toy_builtin.html#use_DirectOutput_Cab_Toys_Hardware_GearMotor", [
        [ "Summary", "toy_builtin.html#use_DirectOutput_Cab_Toys_Hardware_GearMotor_summary", null ],
        [ "Sample XML", "toy_builtin.html#use_DirectOutput_Cab_Toys_Hardware_GearMotor_samplexml", null ],
        [ "Properties", "toy_builtin.html#use_DirectOutput_Cab_Toys_Hardware_GearMotor_properties", [
          [ "FadingCurveName", "toy_builtin.html#DirectOutput_Cab_Toys_Hardware_GearMotor_FadingCurveName", null ],
          [ "KickstartDurationMs", "toy_builtin.html#DirectOutput_Cab_Toys_Hardware_GearMotor_KickstartDurationMs", null ],
          [ "KickstartPower", "toy_builtin.html#DirectOutput_Cab_Toys_Hardware_GearMotor_KickstartPower", null ],
          [ "MaxPower", "toy_builtin.html#DirectOutput_Cab_Toys_Hardware_GearMotor_MaxPower", null ],
          [ "MaxRunTimeMs", "toy_builtin.html#DirectOutput_Cab_Toys_Hardware_GearMotor_MaxRunTimeMs", null ],
          [ "MinPower", "toy_builtin.html#DirectOutput_Cab_Toys_Hardware_GearMotor_MinPower", null ],
          [ "Name", "toy_builtin.html#DirectOutput_Cab_Toys_Hardware_GearMotor_Name", null ],
          [ "OutputName", "toy_builtin.html#DirectOutput_Cab_Toys_Hardware_GearMotor_OutputName", null ]
        ] ]
      ] ],
      [ "Lamp", "toy_builtin.html#use_DirectOutput_Cab_Toys_Hardware_Lamp", [
        [ "Summary", "toy_builtin.html#use_DirectOutput_Cab_Toys_Hardware_Lamp_summary", null ],
        [ "Sample XML", "toy_builtin.html#use_DirectOutput_Cab_Toys_Hardware_Lamp_samplexml", null ],
        [ "Properties", "toy_builtin.html#use_DirectOutput_Cab_Toys_Hardware_Lamp_properties", [
          [ "FadingCurveName", "toy_builtin.html#DirectOutput_Cab_Toys_Hardware_Lamp_FadingCurveName", null ],
          [ "Name", "toy_builtin.html#DirectOutput_Cab_Toys_Hardware_Lamp_Name", null ],
          [ "OutputName", "toy_builtin.html#DirectOutput_Cab_Toys_Hardware_Lamp_OutputName", null ]
        ] ]
      ] ],
      [ "LedStrip", "toy_builtin.html#use_DirectOutput_Cab_Toys_Hardware_LedStrip", [
        [ "Summary", "toy_builtin.html#use_DirectOutput_Cab_Toys_Hardware_LedStrip_summary", null ],
        [ "Sample XML", "toy_builtin.html#use_DirectOutput_Cab_Toys_Hardware_LedStrip_samplexml", null ],
        [ "Properties", "toy_builtin.html#use_DirectOutput_Cab_Toys_Hardware_LedStrip_properties", [
          [ "Brightness", "toy_builtin.html#DirectOutput_Cab_Toys_Hardware_LedStrip_Brightness", null ],
          [ "BrightnessGammaCorrection", "toy_builtin.html#DirectOutput_Cab_Toys_Hardware_LedStrip_BrightnessGammaCorrection", null ],
          [ "ColorOrder", "toy_builtin.html#DirectOutput_Cab_Toys_Hardware_LedStrip_ColorOrder", null ],
          [ "FadingCurveName", "toy_builtin.html#DirectOutput_Cab_Toys_Hardware_LedStrip_FadingCurveName", null ],
          [ "FirstLedNumber", "toy_builtin.html#DirectOutput_Cab_Toys_Hardware_LedStrip_FirstLedNumber", null ],
          [ "Height", "toy_builtin.html#DirectOutput_Cab_Toys_Hardware_LedStrip_Height", null ],
          [ "LedStripArrangement", "toy_builtin.html#DirectOutput_Cab_Toys_Hardware_LedStrip_LedStripArrangement", null ],
          [ "Name", "toy_builtin.html#DirectOutput_Cab_Toys_Hardware_LedStrip_Name", null ],
          [ "OutputControllerName", "toy_builtin.html#DirectOutput_Cab_Toys_Hardware_LedStrip_OutputControllerName", null ],
          [ "Width", "toy_builtin.html#DirectOutput_Cab_Toys_Hardware_LedStrip_Width", null ]
        ] ]
      ] ],
      [ "LedWizEquivalent", "toy_builtin.html#use_DirectOutput_Cab_Toys_LWEquivalent_LedWizEquivalent", [
        [ "Summary", "toy_builtin.html#use_DirectOutput_Cab_Toys_LWEquivalent_LedWizEquivalent_summary", null ],
        [ "Sample XML", "toy_builtin.html#use_DirectOutput_Cab_Toys_LWEquivalent_LedWizEquivalent_samplexml", null ],
        [ "Properties", "toy_builtin.html#use_DirectOutput_Cab_Toys_LWEquivalent_LedWizEquivalent_properties", [
          [ "LedWizNumber", "toy_builtin.html#DirectOutput_Cab_Toys_LWEquivalent_LedWizEquivalent_LedWizNumber", null ],
          [ "Name", "toy_builtin.html#DirectOutput_Cab_Toys_LWEquivalent_LedWizEquivalent_Name", null ],
          [ "Outputs", "toy_builtin.html#DirectOutput_Cab_Toys_LWEquivalent_LedWizEquivalent_Outputs", null ]
        ] ]
      ] ],
      [ "Motor", "toy_builtin.html#use_DirectOutput_Cab_Toys_Hardware_Motor", [
        [ "Summary", "toy_builtin.html#use_DirectOutput_Cab_Toys_Hardware_Motor_summary", null ],
        [ "Sample XML", "toy_builtin.html#use_DirectOutput_Cab_Toys_Hardware_Motor_samplexml", null ],
        [ "Properties", "toy_builtin.html#use_DirectOutput_Cab_Toys_Hardware_Motor_properties", [
          [ "FadingCurveName", "toy_builtin.html#DirectOutput_Cab_Toys_Hardware_Motor_FadingCurveName", null ],
          [ "KickstartDurationMs", "toy_builtin.html#DirectOutput_Cab_Toys_Hardware_Motor_KickstartDurationMs", null ],
          [ "KickstartPower", "toy_builtin.html#DirectOutput_Cab_Toys_Hardware_Motor_KickstartPower", null ],
          [ "MaxPower", "toy_builtin.html#DirectOutput_Cab_Toys_Hardware_Motor_MaxPower", null ],
          [ "MaxRunTimeMs", "toy_builtin.html#DirectOutput_Cab_Toys_Hardware_Motor_MaxRunTimeMs", null ],
          [ "MinPower", "toy_builtin.html#DirectOutput_Cab_Toys_Hardware_Motor_MinPower", null ],
          [ "Name", "toy_builtin.html#DirectOutput_Cab_Toys_Hardware_Motor_Name", null ],
          [ "OutputName", "toy_builtin.html#DirectOutput_Cab_Toys_Hardware_Motor_OutputName", null ]
        ] ]
      ] ],
      [ "RGBAToy", "toy_builtin.html#use_DirectOutput_Cab_Toys_Layer_RGBAToy", [
        [ "Summary", "toy_builtin.html#use_DirectOutput_Cab_Toys_Layer_RGBAToy_summary", null ],
        [ "Sample XML", "toy_builtin.html#use_DirectOutput_Cab_Toys_Layer_RGBAToy_samplexml", null ],
        [ "Properties", "toy_builtin.html#use_DirectOutput_Cab_Toys_Layer_RGBAToy_properties", [
          [ "FadingCurveName", "toy_builtin.html#DirectOutput_Cab_Toys_Layer_RGBAToy_FadingCurveName", null ],
          [ "Name", "toy_builtin.html#DirectOutput_Cab_Toys_Layer_RGBAToy_Name", null ],
          [ "OutputNameBlue", "toy_builtin.html#DirectOutput_Cab_Toys_Layer_RGBAToy_OutputNameBlue", null ],
          [ "OutputNameGreen", "toy_builtin.html#DirectOutput_Cab_Toys_Layer_RGBAToy_OutputNameGreen", null ],
          [ "OutputNameRed", "toy_builtin.html#DirectOutput_Cab_Toys_Layer_RGBAToy_OutputNameRed", null ]
        ] ]
      ] ],
      [ "RGBAToyGroup", "toy_builtin.html#use_DirectOutput_Cab_Toys_Virtual_RGBAToyGroup", [
        [ "Summary", "toy_builtin.html#use_DirectOutput_Cab_Toys_Virtual_RGBAToyGroup_summary", null ],
        [ "Sample XML", "toy_builtin.html#use_DirectOutput_Cab_Toys_Virtual_RGBAToyGroup_samplexml", null ],
        [ "Properties", "toy_builtin.html#use_DirectOutput_Cab_Toys_Virtual_RGBAToyGroup_properties", [
          [ "LayerOffset", "toy_builtin.html#DirectOutput_Cab_Toys_Virtual_RGBAToyGroup_LayerOffset", null ],
          [ "Name", "toy_builtin.html#DirectOutput_Cab_Toys_Virtual_RGBAToyGroup_Name", null ],
          [ "ToyNames", "toy_builtin.html#DirectOutput_Cab_Toys_Virtual_RGBAToyGroup_ToyNames", null ]
        ] ]
      ] ],
      [ "RGBLed", "toy_builtin.html#use_DirectOutput_Cab_Toys_Hardware_RGBLed", [
        [ "Summary", "toy_builtin.html#use_DirectOutput_Cab_Toys_Hardware_RGBLed_summary", null ],
        [ "Sample XML", "toy_builtin.html#use_DirectOutput_Cab_Toys_Hardware_RGBLed_samplexml", null ],
        [ "Properties", "toy_builtin.html#use_DirectOutput_Cab_Toys_Hardware_RGBLed_properties", [
          [ "FadingCurveName", "toy_builtin.html#DirectOutput_Cab_Toys_Hardware_RGBLed_FadingCurveName", null ],
          [ "Name", "toy_builtin.html#DirectOutput_Cab_Toys_Hardware_RGBLed_Name", null ],
          [ "OutputNameBlue", "toy_builtin.html#DirectOutput_Cab_Toys_Hardware_RGBLed_OutputNameBlue", null ],
          [ "OutputNameGreen", "toy_builtin.html#DirectOutput_Cab_Toys_Hardware_RGBLed_OutputNameGreen", null ],
          [ "OutputNameRed", "toy_builtin.html#DirectOutput_Cab_Toys_Hardware_RGBLed_OutputNameRed", null ]
        ] ]
      ] ],
      [ "Shaker", "toy_builtin.html#use_DirectOutput_Cab_Toys_Hardware_Shaker", [
        [ "Summary", "toy_builtin.html#use_DirectOutput_Cab_Toys_Hardware_Shaker_summary", null ],
        [ "Sample XML", "toy_builtin.html#use_DirectOutput_Cab_Toys_Hardware_Shaker_samplexml", null ],
        [ "Properties", "toy_builtin.html#use_DirectOutput_Cab_Toys_Hardware_Shaker_properties", [
          [ "FadingCurveName", "toy_builtin.html#DirectOutput_Cab_Toys_Hardware_Shaker_FadingCurveName", null ],
          [ "KickstartDurationMs", "toy_builtin.html#DirectOutput_Cab_Toys_Hardware_Shaker_KickstartDurationMs", null ],
          [ "KickstartPower", "toy_builtin.html#DirectOutput_Cab_Toys_Hardware_Shaker_KickstartPower", null ],
          [ "MaxPower", "toy_builtin.html#DirectOutput_Cab_Toys_Hardware_Shaker_MaxPower", null ],
          [ "MaxRunTimeMs", "toy_builtin.html#DirectOutput_Cab_Toys_Hardware_Shaker_MaxRunTimeMs", null ],
          [ "MinPower", "toy_builtin.html#DirectOutput_Cab_Toys_Hardware_Shaker_MinPower", null ],
          [ "Name", "toy_builtin.html#DirectOutput_Cab_Toys_Hardware_Shaker_Name", null ],
          [ "OutputName", "toy_builtin.html#DirectOutput_Cab_Toys_Hardware_Shaker_OutputName", null ]
        ] ]
      ] ]
    ] ],
    [ "Output controllers", "outputcontrollers.html", [
      [ "Introduction", "outputcontrollers.html#outputcontrollers_introduction", null ],
      [ "Builtin output controllers", "outputcontrollers.html#outputcontrollers_builtincontrollers", [
        [ "Interfaces for output controllers", "outputcontrollers.html#outputcontrollers_interfaces", [
          [ "IOutputController interface", "outputcontrollers.html#outputcontrollers_ioutputcontroller", null ],
          [ "IOutput interface", "outputcontrollers.html#outputcontrollers_ioutput", null ]
        ] ],
        [ "Implementation guidelines for custom output controllers", "outputcontrollers.html#outputcontrollers_implementationguideline", null ]
      ] ]
    ] ],
    [ "Built in Output controllers", "outputcontrollers_builtin.html", [
      [ "ArtNet", "outputcontrollers_builtin.html#use_DirectOutput_Cab_Out_DMX_ArtNet", [
        [ "Summary", "outputcontrollers_builtin.html#use_DirectOutput_Cab_Out_DMX_ArtNet_summary", null ],
        [ "Sample XML", "outputcontrollers_builtin.html#use_DirectOutput_Cab_Out_DMX_ArtNet_samplexml", null ],
        [ "Properties", "outputcontrollers_builtin.html#use_DirectOutput_Cab_Out_DMX_ArtNet_properties", [
          [ "BroadcastAddress", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_DMX_ArtNet_BroadcastAddress", null ],
          [ "Name", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_DMX_ArtNet_Name", null ],
          [ "Universe", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_DMX_ArtNet_Universe", null ]
        ] ]
      ] ],
      [ "DirectStripController", "outputcontrollers_builtin.html#use_DirectOutput_Cab_Out_AdressableLedStrip_DirectStripController", [
        [ "Summary", "outputcontrollers_builtin.html#use_DirectOutput_Cab_Out_AdressableLedStrip_DirectStripController_summary", null ],
        [ "Sample XML", "outputcontrollers_builtin.html#use_DirectOutput_Cab_Out_AdressableLedStrip_DirectStripController_samplexml", null ],
        [ "Properties", "outputcontrollers_builtin.html#use_DirectOutput_Cab_Out_AdressableLedStrip_DirectStripController_properties", [
          [ "ControllerNumber", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_AdressableLedStrip_DirectStripController_ControllerNumber", null ],
          [ "Name", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_AdressableLedStrip_DirectStripController_Name", null ],
          [ "NumberOfLeds", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_AdressableLedStrip_DirectStripController_NumberOfLeds", null ],
          [ "PackData", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_AdressableLedStrip_DirectStripController_PackData", null ]
        ] ]
      ] ],
      [ "DudesCab", "outputcontrollers_builtin.html#use_DirectOutput_Cab_Out_DudesCab_DudesCab", [
        [ "Summary", "outputcontrollers_builtin.html#use_DirectOutput_Cab_Out_DudesCab_DudesCab_summary", null ],
        [ "Sample XML", "outputcontrollers_builtin.html#use_DirectOutput_Cab_Out_DudesCab_DudesCab_samplexml", null ],
        [ "Properties", "outputcontrollers_builtin.html#use_DirectOutput_Cab_Out_DudesCab_DudesCab_properties", [
          [ "MinCommandIntervalMs", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_DudesCab_DudesCab_MinCommandIntervalMs", null ],
          [ "Name", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_DudesCab_DudesCab_Name", null ],
          [ "Number", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_DudesCab_DudesCab_Number", null ],
          [ "NumberOfOutputs", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_DudesCab_DudesCab_NumberOfOutputs", null ]
        ] ]
      ] ],
      [ "FT245RBitbangController", "outputcontrollers_builtin.html#use_DirectOutput_Cab_Out_FTDIChip_FT245RBitbangController", [
        [ "Summary", "outputcontrollers_builtin.html#use_DirectOutput_Cab_Out_FTDIChip_FT245RBitbangController_summary", null ],
        [ "Sample XML", "outputcontrollers_builtin.html#use_DirectOutput_Cab_Out_FTDIChip_FT245RBitbangController_samplexml", null ],
        [ "Properties", "outputcontrollers_builtin.html#use_DirectOutput_Cab_Out_FTDIChip_FT245RBitbangController_properties", [
          [ "Description", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_FTDIChip_FT245RBitbangController_Description", null ],
          [ "Id", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_FTDIChip_FT245RBitbangController_Id", null ],
          [ "Name", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_FTDIChip_FT245RBitbangController_Name", null ],
          [ "SerialNumber", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_FTDIChip_FT245RBitbangController_SerialNumber", null ]
        ] ]
      ] ],
      [ "LedWiz", "outputcontrollers_builtin.html#use_DirectOutput_Cab_Out_LW_LedWiz", [
        [ "Summary", "outputcontrollers_builtin.html#use_DirectOutput_Cab_Out_LW_LedWiz_summary", null ],
        [ "Sample XML", "outputcontrollers_builtin.html#use_DirectOutput_Cab_Out_LW_LedWiz_samplexml", null ],
        [ "Properties", "outputcontrollers_builtin.html#use_DirectOutput_Cab_Out_LW_LedWiz_properties", [
          [ "MinCommandIntervalMs", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_LW_LedWiz_MinCommandIntervalMs", null ],
          [ "Name", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_LW_LedWiz_Name", null ],
          [ "Number", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_LW_LedWiz_Number", null ]
        ] ]
      ] ],
      [ "NullOutputController", "outputcontrollers_builtin.html#use_DirectOutput_Cab_Out_NullOutputController_NullOutputController", [
        [ "Summary", "outputcontrollers_builtin.html#use_DirectOutput_Cab_Out_NullOutputController_NullOutputController_summary", null ],
        [ "Sample XML", "outputcontrollers_builtin.html#use_DirectOutput_Cab_Out_NullOutputController_NullOutputController_samplexml", null ],
        [ "Properties", "outputcontrollers_builtin.html#use_DirectOutput_Cab_Out_NullOutputController_NullOutputController_properties", [
          [ "Name", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_NullOutputController_NullOutputController_Name", null ]
        ] ]
      ] ],
      [ "PacDrive", "outputcontrollers_builtin.html#use_DirectOutput_Cab_Out_Pac_PacDrive", [
        [ "Summary", "outputcontrollers_builtin.html#use_DirectOutput_Cab_Out_Pac_PacDrive_summary", null ],
        [ "Sample XML", "outputcontrollers_builtin.html#use_DirectOutput_Cab_Out_Pac_PacDrive_samplexml", null ],
        [ "Properties", "outputcontrollers_builtin.html#use_DirectOutput_Cab_Out_Pac_PacDrive_properties", [
          [ "Name", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_Pac_PacDrive_Name", null ]
        ] ]
      ] ],
      [ "PacLed64", "outputcontrollers_builtin.html#use_DirectOutput_Cab_Out_Pac_PacLed64", [
        [ "Summary", "outputcontrollers_builtin.html#use_DirectOutput_Cab_Out_Pac_PacLed64_summary", null ],
        [ "Sample XML", "outputcontrollers_builtin.html#use_DirectOutput_Cab_Out_Pac_PacLed64_samplexml", null ],
        [ "Properties", "outputcontrollers_builtin.html#use_DirectOutput_Cab_Out_Pac_PacLed64_properties", [
          [ "FullUpdateThreshold", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_Pac_PacLed64_FullUpdateThreshold", null ],
          [ "Id", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_Pac_PacLed64_Id", null ],
          [ "MinUpdateIntervalMs", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_Pac_PacLed64_MinUpdateIntervalMs", null ],
          [ "Name", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_Pac_PacLed64_Name", null ]
        ] ]
      ] ],
      [ "PacUIO", "outputcontrollers_builtin.html#use_DirectOutput_Cab_Out_Pac_PacUIO", [
        [ "Summary", "outputcontrollers_builtin.html#use_DirectOutput_Cab_Out_Pac_PacUIO_summary", null ],
        [ "Sample XML", "outputcontrollers_builtin.html#use_DirectOutput_Cab_Out_Pac_PacUIO_samplexml", null ],
        [ "Properties", "outputcontrollers_builtin.html#use_DirectOutput_Cab_Out_Pac_PacUIO_properties", [
          [ "Id", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_Pac_PacUIO_Id", null ],
          [ "Name", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_Pac_PacUIO_Name", null ]
        ] ]
      ] ],
      [ "PhilipsHueController", "outputcontrollers_builtin.html#use_DirectOutput_Cab_Out_Pac_PhilipsHueController", [
        [ "Summary", "outputcontrollers_builtin.html#use_DirectOutput_Cab_Out_Pac_PhilipsHueController_summary", null ],
        [ "Sample XML", "outputcontrollers_builtin.html#use_DirectOutput_Cab_Out_Pac_PhilipsHueController_samplexml", null ],
        [ "Properties", "outputcontrollers_builtin.html#use_DirectOutput_Cab_Out_Pac_PhilipsHueController_properties", [
          [ "BridgeDeviceType", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_Pac_PhilipsHueController_BridgeDeviceType", null ],
          [ "BridgeIP", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_Pac_PhilipsHueController_BridgeIP", null ],
          [ "BridgeKey", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_Pac_PhilipsHueController_BridgeKey", null ],
          [ "Id", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_Pac_PhilipsHueController_Id", null ],
          [ "Name", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_Pac_PhilipsHueController_Name", null ]
        ] ]
      ] ],
      [ "PinControl", "outputcontrollers_builtin.html#use_DirectOutput_Cab_Out_ComPort_PinControl", [
        [ "Summary", "outputcontrollers_builtin.html#use_DirectOutput_Cab_Out_ComPort_PinControl_summary", null ],
        [ "Sample XML", "outputcontrollers_builtin.html#use_DirectOutput_Cab_Out_ComPort_PinControl_samplexml", null ],
        [ "Properties", "outputcontrollers_builtin.html#use_DirectOutput_Cab_Out_ComPort_PinControl_properties", [
          [ "ComPort", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_ComPort_PinControl_ComPort", null ],
          [ "Name", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_ComPort_PinControl_Name", null ]
        ] ]
      ] ],
      [ "PinOne", "outputcontrollers_builtin.html#use_DirectOutput_Cab_Out_PinOne_PinOne", [
        [ "Summary", "outputcontrollers_builtin.html#use_DirectOutput_Cab_Out_PinOne_PinOne_summary", null ],
        [ "Sample XML", "outputcontrollers_builtin.html#use_DirectOutput_Cab_Out_PinOne_PinOne_samplexml", null ],
        [ "Properties", "outputcontrollers_builtin.html#use_DirectOutput_Cab_Out_PinOne_PinOne_properties", [
          [ "ComPort", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_PinOne_PinOne_ComPort", null ],
          [ "MinCommandIntervalMs", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_PinOne_PinOne_MinCommandIntervalMs", null ],
          [ "Name", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_PinOne_PinOne_Name", null ],
          [ "Number", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_PinOne_PinOne_Number", null ],
          [ "NumberOfOutputs", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_PinOne_PinOne_NumberOfOutputs", null ]
        ] ]
      ] ],
      [ "Pinscape", "outputcontrollers_builtin.html#use_DirectOutput_Cab_Out_PS_Pinscape", [
        [ "Summary", "outputcontrollers_builtin.html#use_DirectOutput_Cab_Out_PS_Pinscape_summary", null ],
        [ "Sample XML", "outputcontrollers_builtin.html#use_DirectOutput_Cab_Out_PS_Pinscape_samplexml", null ],
        [ "Properties", "outputcontrollers_builtin.html#use_DirectOutput_Cab_Out_PS_Pinscape_properties", [
          [ "MinCommandIntervalMs", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_PS_Pinscape_MinCommandIntervalMs", null ],
          [ "Name", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_PS_Pinscape_Name", null ],
          [ "Number", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_PS_Pinscape_Number", null ],
          [ "NumberOfOutputs", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_PS_Pinscape_NumberOfOutputs", null ]
        ] ]
      ] ],
      [ "PinscapePico", "outputcontrollers_builtin.html#use_DirectOutput_Cab_Out_PSPico_PinscapePico", [
        [ "Summary", "outputcontrollers_builtin.html#use_DirectOutput_Cab_Out_PSPico_PinscapePico_summary", null ],
        [ "Sample XML", "outputcontrollers_builtin.html#use_DirectOutput_Cab_Out_PSPico_PinscapePico_samplexml", null ],
        [ "Properties", "outputcontrollers_builtin.html#use_DirectOutput_Cab_Out_PSPico_PinscapePico_properties", [
          [ "MinCommandIntervalMs", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_PSPico_PinscapePico_MinCommandIntervalMs", null ],
          [ "Name", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_PSPico_PinscapePico_Name", null ],
          [ "Number", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_PSPico_PinscapePico_Number", null ],
          [ "NumberOfOutputs", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_PSPico_PinscapePico_NumberOfOutputs", null ]
        ] ]
      ] ],
      [ "SSFImpactController", "outputcontrollers_builtin.html#use_DirectOutput_Cab_Out_SSFImpactController_SSFImpactController", [
        [ "Summary", "outputcontrollers_builtin.html#use_DirectOutput_Cab_Out_SSFImpactController_SSFImpactController_summary", null ],
        [ "Sample XML", "outputcontrollers_builtin.html#use_DirectOutput_Cab_Out_SSFImpactController_SSFImpactController_samplexml", null ],
        [ "Properties", "outputcontrollers_builtin.html#use_DirectOutput_Cab_Out_SSFImpactController_SSFImpactController_properties", [
          [ "BassShaker1", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_SSFImpactController_SSFImpactController_BassShaker1", null ],
          [ "BassShaker2", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_SSFImpactController_SSFImpactController_BassShaker2", null ],
          [ "BumperLevel", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_SSFImpactController_SSFImpactController_BumperLevel", null ],
          [ "DeviceNumber", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_SSFImpactController_SSFImpactController_DeviceNumber", null ],
          [ "FlipperLevel", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_SSFImpactController_SSFImpactController_FlipperLevel", null ],
          [ "FrontExciters", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_SSFImpactController_SSFImpactController_FrontExciters", null ],
          [ "GearLevel", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_SSFImpactController_SSFImpactController_GearLevel", null ],
          [ "ImpactFactor", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_SSFImpactController_SSFImpactController_ImpactFactor", null ],
          [ "LowImpactMode", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_SSFImpactController_SSFImpactController_LowImpactMode", null ],
          [ "Name", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_SSFImpactController_SSFImpactController_Name", null ],
          [ "RearExciters", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_SSFImpactController_SSFImpactController_RearExciters", null ],
          [ "ShakerImpactFactor", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_SSFImpactController_SSFImpactController_ShakerImpactFactor", null ],
          [ "SlingsLevel", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_SSFImpactController_SSFImpactController_SlingsLevel", null ]
        ] ]
      ] ],
      [ "TeensyStripController", "outputcontrollers_builtin.html#use_DirectOutput_Cab_Out_AdressableLedStrip_TeensyStripController", [
        [ "Summary", "outputcontrollers_builtin.html#use_DirectOutput_Cab_Out_AdressableLedStrip_TeensyStripController_summary", null ],
        [ "Sample XML", "outputcontrollers_builtin.html#use_DirectOutput_Cab_Out_AdressableLedStrip_TeensyStripController_samplexml", null ],
        [ "Properties", "outputcontrollers_builtin.html#use_DirectOutput_Cab_Out_AdressableLedStrip_TeensyStripController_properties", [
          [ "ComPortBaudRate", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_AdressableLedStrip_TeensyStripController_ComPortBaudRate", null ],
          [ "ComPortDataBits", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_AdressableLedStrip_TeensyStripController_ComPortDataBits", null ],
          [ "ComPortDtrEnable", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_AdressableLedStrip_TeensyStripController_ComPortDtrEnable", null ],
          [ "ComPortHandshakeEndWaitMs", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_AdressableLedStrip_TeensyStripController_ComPortHandshakeEndWaitMs", null ],
          [ "ComPortHandshakeStartWaitMs", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_AdressableLedStrip_TeensyStripController_ComPortHandshakeStartWaitMs", null ],
          [ "ComPortName", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_AdressableLedStrip_TeensyStripController_ComPortName", null ],
          [ "ComPortOpenWaitMs", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_AdressableLedStrip_TeensyStripController_ComPortOpenWaitMs", null ],
          [ "ComPortParity", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_AdressableLedStrip_TeensyStripController_ComPortParity", null ],
          [ "ComPortStopBits", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_AdressableLedStrip_TeensyStripController_ComPortStopBits", null ],
          [ "ComPortTimeOutMs", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_AdressableLedStrip_TeensyStripController_ComPortTimeOutMs", null ],
          [ "Name", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_AdressableLedStrip_TeensyStripController_Name", null ],
          [ "NumberOfLedsStrip1", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_AdressableLedStrip_TeensyStripController_NumberOfLedsStrip1", null ],
          [ "NumberOfLedsStrip10", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_AdressableLedStrip_TeensyStripController_NumberOfLedsStrip10", null ],
          [ "NumberOfLedsStrip2", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_AdressableLedStrip_TeensyStripController_NumberOfLedsStrip2", null ],
          [ "NumberOfLedsStrip3", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_AdressableLedStrip_TeensyStripController_NumberOfLedsStrip3", null ],
          [ "NumberOfLedsStrip4", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_AdressableLedStrip_TeensyStripController_NumberOfLedsStrip4", null ],
          [ "NumberOfLedsStrip5", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_AdressableLedStrip_TeensyStripController_NumberOfLedsStrip5", null ],
          [ "NumberOfLedsStrip6", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_AdressableLedStrip_TeensyStripController_NumberOfLedsStrip6", null ],
          [ "NumberOfLedsStrip7", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_AdressableLedStrip_TeensyStripController_NumberOfLedsStrip7", null ],
          [ "NumberOfLedsStrip8", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_AdressableLedStrip_TeensyStripController_NumberOfLedsStrip8", null ],
          [ "NumberOfLedsStrip9", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_AdressableLedStrip_TeensyStripController_NumberOfLedsStrip9", null ]
        ] ]
      ] ],
      [ "WemosD1MPStripController", "outputcontrollers_builtin.html#use_DirectOutput_Cab_Out_AdressableLedStrip_WemosD1MPStripController", [
        [ "Summary", "outputcontrollers_builtin.html#use_DirectOutput_Cab_Out_AdressableLedStrip_WemosD1MPStripController_summary", null ],
        [ "Sample XML", "outputcontrollers_builtin.html#use_DirectOutput_Cab_Out_AdressableLedStrip_WemosD1MPStripController_samplexml", null ],
        [ "Properties", "outputcontrollers_builtin.html#use_DirectOutput_Cab_Out_AdressableLedStrip_WemosD1MPStripController_properties", [
          [ "ComPortBaudRate", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_AdressableLedStrip_WemosD1MPStripController_ComPortBaudRate", null ],
          [ "ComPortDataBits", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_AdressableLedStrip_WemosD1MPStripController_ComPortDataBits", null ],
          [ "ComPortDtrEnable", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_AdressableLedStrip_WemosD1MPStripController_ComPortDtrEnable", null ],
          [ "ComPortHandshakeEndWaitMs", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_AdressableLedStrip_WemosD1MPStripController_ComPortHandshakeEndWaitMs", null ],
          [ "ComPortHandshakeStartWaitMs", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_AdressableLedStrip_WemosD1MPStripController_ComPortHandshakeStartWaitMs", null ],
          [ "ComPortName", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_AdressableLedStrip_WemosD1MPStripController_ComPortName", null ],
          [ "ComPortOpenWaitMs", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_AdressableLedStrip_WemosD1MPStripController_ComPortOpenWaitMs", null ],
          [ "ComPortParity", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_AdressableLedStrip_WemosD1MPStripController_ComPortParity", null ],
          [ "ComPortStopBits", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_AdressableLedStrip_WemosD1MPStripController_ComPortStopBits", null ],
          [ "ComPortTimeOutMs", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_AdressableLedStrip_WemosD1MPStripController_ComPortTimeOutMs", null ],
          [ "Name", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_AdressableLedStrip_WemosD1MPStripController_Name", null ],
          [ "NumberOfLedsStrip1", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_AdressableLedStrip_WemosD1MPStripController_NumberOfLedsStrip1", null ],
          [ "NumberOfLedsStrip10", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_AdressableLedStrip_WemosD1MPStripController_NumberOfLedsStrip10", null ],
          [ "NumberOfLedsStrip2", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_AdressableLedStrip_WemosD1MPStripController_NumberOfLedsStrip2", null ],
          [ "NumberOfLedsStrip3", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_AdressableLedStrip_WemosD1MPStripController_NumberOfLedsStrip3", null ],
          [ "NumberOfLedsStrip4", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_AdressableLedStrip_WemosD1MPStripController_NumberOfLedsStrip4", null ],
          [ "NumberOfLedsStrip5", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_AdressableLedStrip_WemosD1MPStripController_NumberOfLedsStrip5", null ],
          [ "NumberOfLedsStrip6", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_AdressableLedStrip_WemosD1MPStripController_NumberOfLedsStrip6", null ],
          [ "NumberOfLedsStrip7", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_AdressableLedStrip_WemosD1MPStripController_NumberOfLedsStrip7", null ],
          [ "NumberOfLedsStrip8", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_AdressableLedStrip_WemosD1MPStripController_NumberOfLedsStrip8", null ],
          [ "NumberOfLedsStrip9", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_AdressableLedStrip_WemosD1MPStripController_NumberOfLedsStrip9", null ],
          [ "SendPerLedstripLength", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_AdressableLedStrip_WemosD1MPStripController_SendPerLedstripLength", null ],
          [ "TestOnConnect", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_AdressableLedStrip_WemosD1MPStripController_TestOnConnect", null ],
          [ "UseCompression", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_AdressableLedStrip_WemosD1MPStripController_UseCompression", null ]
        ] ]
      ] ],
      [ "WS2811StripController", "outputcontrollers_builtin.html#use_DirectOutput_Cab_Out_AdressableLedStrip_WS2811StripController", [
        [ "Summary", "outputcontrollers_builtin.html#use_DirectOutput_Cab_Out_AdressableLedStrip_WS2811StripController_summary", null ],
        [ "Sample XML", "outputcontrollers_builtin.html#use_DirectOutput_Cab_Out_AdressableLedStrip_WS2811StripController_samplexml", null ],
        [ "Properties", "outputcontrollers_builtin.html#use_DirectOutput_Cab_Out_AdressableLedStrip_WS2811StripController_properties", [
          [ "ControllerNumber", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_AdressableLedStrip_WS2811StripController_ControllerNumber", null ],
          [ "Name", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_AdressableLedStrip_WS2811StripController_Name", null ],
          [ "NumberOfLeds", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_AdressableLedStrip_WS2811StripController_NumberOfLeds", null ],
          [ "PackData", "outputcontrollers_builtin.html#DirectOutput_Cab_Out_AdressableLedStrip_WS2811StripController_PackData", null ]
        ] ]
      ] ]
    ] ],
    [ "Source Code", "sourcecode.html", [
      [ "Introduction", "sourcecode.html#sourcecode_introduction", null ],
      [ "Download source code/Fork source code", "sourcecode.html#sourcecode_fork", null ],
      [ "Contribute your code/Pull requests", "sourcecode.html#sourcecode_pull", null ],
      [ "Tools", "sourcecode.html#sourcecode_tools", null ],
      [ "Using the source code", "sourcecode.html#sourcecode_use", null ],
      [ "What is where", "sourcecode.html#sourcecode_where", [
        [ "DirectOutput project", "sourcecode.html#sourcecode_wheredirectoutput", null ],
        [ "Documentation project", "sourcecode.html#sourcecode_wheredocu", null ],
        [ "B2SServerPlugin project", "sourcecode.html#sourcecode_whereplugin", null ],
        [ "LedControlFileTester project", "sourcecode.html#sourcecode_wheredplugin", null ]
      ] ],
      [ "Extending and changing the source code (guidelines)", "sourcecode.html#sourcecode_extend", null ]
    ] ],
    [ "Support", "support.html", null ],
    [ "Contribute", "contribute.html", [
      [ "Report bugs and issues", "contribute.html#contribute_bugs", null ],
      [ "Important!", "contribute.html#autotoc_md0", null ],
      [ "Squash some bugs", "contribute.html#contribute_squashbugs", null ],
      [ "Share your knowledge", "contribute.html#contribute_help", null ],
      [ "Create more effects", "contribute.html#autotoc_md1", null ],
      [ "Add more and better toys", "contribute.html#autotoc_md2", null ],
      [ "Add more output controllers", "contribute.html#autotoc_md3", null ],
      [ "Contribute the to documentation", "contribute.html#contribute_docu", null ],
      [ "Contribute to the source code", "contribute.html#contribute_source", null ]
    ] ],
    [ "Downloads", "downloads.html", [
      [ "Binary Package", "downloads.html#download_binaries", null ],
      [ "Source Code", "downloads.html#downloads_sourcecode", null ],
      [ "Documentation", "downloads.html#downloads_docu", null ],
      [ "Table config files", "downloads.html#downloads_config", null ]
    ] ],
    [ "DOF History", "dofhistory.html", null ],
    [ "Credits", "md__d_1_2a_2_direct_output_2_direct_output_2_documentation_290___credits.html", [
      [ "B2S Direct BackglassServer", "md__d_1_2a_2_direct_output_2_direct_output_2_documentation_290___credits.html#autotoc_md4", null ],
      [ "Testing and support", "md__d_1_2a_2_direct_output_2_direct_output_2_documentation_290___credits.html#autotoc_md5", null ],
      [ "Table configuration and config tool", "md__d_1_2a_2_direct_output_2_direct_output_2_documentation_290___credits.html#autotoc_md6", null ],
      [ "GroovyGameGear/LedWiz", "md__d_1_2a_2_direct_output_2_direct_output_2_documentation_290___credits.html#autotoc_md7", null ],
      [ "Ultimarc/PacDrive/PacLed64", "md__d_1_2a_2_direct_output_2_direct_output_2_documentation_290___credits.html#autotoc_md8", null ],
      [ "SainSmart support", "md__d_1_2a_2_direct_output_2_direct_output_2_documentation_290___credits.html#autotoc_md9", null ],
      [ "CSScript", "md__d_1_2a_2_direct_output_2_direct_output_2_documentation_290___credits.html#autotoc_md10", null ]
    ] ],
    [ "About and Copyright", "md__d_1_2a_2_direct_output_2_direct_output_2_documentation_299___mainpage_and_about.html", null ],
    [ "DOF Build Setup", "md__d_1_2a_2_direct_output_2_direct_output_2_r_e_a_d_m_e___build_setup.html", [
      [ "Migrating to newer Visual Studio versions", "md__d_1_2a_2_direct_output_2_direct_output_2_r_e_a_d_m_e___build_setup.html#autotoc_md21", null ],
      [ "WiX setup (optional - required only for .msi creation)", "md__d_1_2a_2_direct_output_2_direct_output_2_r_e_a_d_m_e___build_setup.html#autotoc_md22", null ],
      [ "Build procedure", "md__d_1_2a_2_direct_output_2_direct_output_2_r_e_a_d_m_e___build_setup.html#autotoc_md23", null ],
      [ "Release process", "md__d_1_2a_2_direct_output_2_direct_output_2_r_e_a_d_m_e___build_setup.html#autotoc_md24", [
        [ "New single-script release builder", "md__d_1_2a_2_direct_output_2_direct_output_2_r_e_a_d_m_e___build_setup.html#autotoc_md25", null ],
        [ "Original manual release process", "md__d_1_2a_2_direct_output_2_direct_output_2_r_e_a_d_m_e___build_setup.html#autotoc_md26", null ]
      ] ]
    ] ],
    [ "Packages", "namespaces.html", [
      [ "Package List", "namespaces.html", "namespaces_dup" ],
      [ "Package Members", "namespacemembers.html", [
        [ "All", "namespacemembers.html", null ],
        [ "Functions", "namespacemembers_func.html", null ],
        [ "Enumerations", "namespacemembers_enum.html", null ]
      ] ]
    ] ],
    [ "Classes", "annotated.html", [
      [ "Class List", "annotated.html", "annotated_dup" ],
      [ "Class Index", "classes.html", null ],
      [ "Class Hierarchy", "hierarchy.html", "hierarchy" ],
      [ "Class Members", "functions.html", [
        [ "All", "functions.html", "functions_dup" ],
        [ "Functions", "functions_func.html", "functions_func" ],
        [ "Variables", "functions_vars.html", "functions_vars" ],
        [ "Enumerations", "functions_enum.html", null ],
        [ "Properties", "functions_prop.html", "functions_prop" ],
        [ "Events", "functions_evnt.html", null ]
      ] ]
    ] ],
    [ "Files", "files.html", [
      [ "File List", "files.html", "files_dup" ],
      [ "File Members", "globals.html", [
        [ "All", "globals.html", null ],
        [ "Variables", "globals_vars.html", null ],
        [ "Typedefs", "globals_type.html", null ],
        [ "Enumerations", "globals_enum.html", null ],
        [ "Enumerator", "globals_eval.html", null ]
      ] ]
    ] ]
  ] ]
];

var NAVTREEINDEX =
[
"_adressable_led_strip_8cs.html",
"_pin_one_8cs.html",
"class_direct_output_1_1_cab_1_1_cabinet_output_list.html#a303e224af289c90796c50169678cb726",
"class_direct_output_1_1_cab_1_1_out_1_1_dudes_cab_1_1_dudes_cab_1_1_device.html#a7f6690008f989d3ab45bd3c07db9f59b",
"class_direct_output_1_1_cab_1_1_out_1_1_f_t_d_i_chip_1_1_f_t_d_i.html#af6622f69a90ea6c9ce1a6a7d81ea9297",
"class_direct_output_1_1_cab_1_1_out_1_1_f_t_d_i_chip_1_1_f_t_d_i_1_1_f_t___d_e_v_i_c_e___i_n_f_o___n_o_d_e.html#a81b93d8671f8f01eba209fe14bb2f35b",
"class_direct_output_1_1_cab_1_1_out_1_1_output_controller_flex_complete_base.html#afcc55d3179a389da77c4cb0d498388ec",
"class_direct_output_1_1_cab_1_1_out_1_1_pac_1_1_pac_drive_singleton.html#a89e84a81f300b781f26b5ba8acd84adf",
"class_direct_output_1_1_cab_1_1_overrides_1_1_table_override_setting.html",
"class_direct_output_1_1_cab_1_1_toys_1_1_l_w_equivalent_1_1_led_wiz_equivalent_output.html#a2be5a667f8370bc1695d9c660f88e6d4",
"class_direct_output_1_1_f_x_1_1_effect_event_args.html",
"class_direct_output_1_1_f_x_1_1_matrix_f_x_1_1_matrix_shift_effect_base-1-g.html",
"class_direct_output_1_1_frontend_1_1_cabinet_info.html#a84aa5bcda872e332040e4c28e69e740e",
"class_direct_output_1_1_general_1_1_file_pattern.html#a8469bcb2386dc7e6314195baf7940fb9",
"class_direct_output_1_1_general_1_1_type_list.html#af93962c9dca8e4911851a9affa9f7875",
"class_direct_output_1_1_led_control_1_1_loader_1_1_table_config_setting.html#abfe64a0a46fc2a6ba76141cc83092148",
"class_direct_output_1_1_table_1_1_table_element_value_changed_event_args.html#a3e8bea22938630008b854981f7027e15",
"class_pinball_x_1_1_plugin.html#a7b11cdda4cb398174a46974fb6d4e1de",
"functions_vars_l.html",
"fx_builtin.html#DirectOutput_FX_NullFX_NullEffect_Name",
"interface_direct_output_1_1_cab_1_1_out_1_1_i_supports_set_values.html#abbac41c47b8dde231fba7f8919a69e69",
"namespace_direct_output_1_1_general_1_1_bitmap_handling.html#a671b0c984f3e1832b0fea3c72b88a8c5a19832d68a379463040c1fd3f6157a066",
"pbx.html",
"toy_builtin.html#use_DirectOutput_Cab_Toys_Hardware_Shaker_properties"
];

var SYNCONMSG = 'click to disable panel synchronization';
var SYNCOFFMSG = 'click to enable panel synchronization';
var LISTOFALLMEMBERS = 'List of all members';