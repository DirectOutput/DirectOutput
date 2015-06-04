Imports System.Runtime.InteropServices
Imports DirectOutput
Imports System.IO
Imports System.Reflection

''' <summary>
''' A simple com object implementation for DirectOutput.<br/>
''' It is in vb.net, since there are not templates to create com objects in C#.<br/>
''' Do I have to mention that IMHO vb.net is a real nightmare to work with.
''' </summary>
<ProgId("DirectOutput.ComObject"), ComClass(ComObject.ClassId, ComObject.InterfaceId, ComObject.EventsId)> _
Public Class ComObject

    Implements IDisposable

#Region "COM-GUIDs"
    ' Diese GUIDs stellen die COM-Identität für diese Klasse 
    ' und ihre COM-Schnittstellen bereit. Wenn Sie sie ändern, können vorhandene 
    ' Clients nicht mehr auf die Klasse zugreifen.
    Public Const ClassId As String = "a23bfdbc-9a8a-46c0-8672-60f23d54ffb6"
    Public Const InterfaceId As String = "63dc1112-571f-4a49-b2fd-cf98c02bf5d4"
    Public Const EventsId As String = "a5ff940d-41d4-4dad-80af-4688e3f737c1"
#End Region


    Public Sub New()
        MyBase.New()
    End Sub


    ''' <summary>
    ''' Gets the version of the DirectOutput framework.
    ''' </summary>
    ''' <returns>String containing the version of the framework.</returns>
    Function GetVersion() As String
        Dim V As Version = GetType(Pinball).Assembly.GetName().Version
        Return V.ToString()
    End Function


    ''' <summary>
    ''' Gets the name and the version of the DirectOutput framework.
    ''' </summary>
    ''' <returns>String containg the name and the version of the DirectOutput framework.</returns>
    Function GetName() As String
        Dim V As Version = GetType(Pinball).Assembly.GetName().Version
        Dim BuildDate As DateTime = New DateTime(2000, 1, 1).AddDays(V.Build).AddSeconds(V.Revision * 2)
        Return "DirectOutput (V: {0} as of {1})".Build(V.ToString(), BuildDate.ToString("yyyy.MM.dd HH:mm"))
    End Function


    ''' <summary>
    ''' Gets the path to the directory of the DirectOutput framework.
    ''' </summary>
    ''' <returns></returns>
    Function GetDllPath() As String
        Return New FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName

    End Function


    ''' <summary>
    ''' Finishes the DirectOutput framework.
    ''' </summary>
    Public Sub Finish()

        If Not Pinball Is Nothing Then
            Pinball.Finish()
            Pinball = Nothing
        End If

    End Sub

    ''' <summary>
    ''' Updates the data of a table element
    ''' </summary>
    ''' <param name="TableElementTypeChar">The table element type char.<br/>Only the first letter/character of this para is passed on to the destination.</param>
    ''' <param name="Number">The number of the table element.</param>
    ''' <param name="Value">The value of the table element.</param>
    ''' <exception cref="System.Exception">
    ''' Could not extract the first char of the TableElementTypeChar parameter
    ''' or
    ''' You must call Init before sending data.
    ''' </exception>
    Public Sub UpdateTableElement(TableElementTypeChar As String, Number As Integer, Value As Integer)
        Dim C As Char
        Try
            C = TableElementTypeChar.Substring(0, 1)
        Catch ex As Exception
            Throw New Exception("Could not extract the first char of the TableElementTypeChar parameter", ex)
        End Try
        If Pinball IsNot Nothing Then
            Pinball.ReceiveData(C, Number, Value)
        Else
            Throw New Exception("You must call Init before sending data.")
        End If

    End Sub


    ''' <summary>
    ''' Updates a named table element.
    ''' </summary>
    ''' <param name="TableElementName">Name of the table element.</param>
    ''' <param name="Value">The value of the table element.</param>
    ''' <exception cref="System.ArgumentException">The TableElementName cant be null or empty.</exception>
    ''' <exception cref="System.Exception">You must call Init before sending data.</exception>
    Public Sub UpdateNamedTableElement(TableElementName As String, Value As Integer)

        If TableElementName.IsNullOrWhiteSpace() Then
            Throw New ArgumentException("The TableElementName cant be null or empty.", "TableElementName")
        End If
        If Pinball IsNot Nothing Then
            Pinball.ReceiveData(TableElementName, Value)
        Else
            Throw New Exception("You must call Init before sending data.")
        End If


    End Sub



    ''' <summary>
    ''' Initializes the DirectOutput framework.<br/>
    ''' \remark: The DirectOutput framework uses the HostingApplicationName parameter to construct the name of the global config file. The name of the global config file is GlobalConfig_{HostingApplicationName}.xml<br/>
    ''' For more information on global configuration check the page on this topic in the documentation.
    ''' </summary>
    ''' <param name="HostingApplicationName">Name of the hosting application. The parameter is used to contruct the name of the global config file for which the framework is looking.</param>
    ''' <param name="TableFileName">Name of the table file.<br/>If there is no table file available, it is recommended to supply the name and path of a not existing, dummy table file since the system can use this to determine the location of config files (depends on global config settings)</param>
    ''' <param name="GameName">Name of the game / rom name. <br/>If no rom name is available (e.g. for EM tables) you can supply a string which identifies the game uniquely. This para can be used to lookup table configs (depends on global config and the other config files).</param>
    ''' <exception cref="System.Exception">Object has already been initialized. Call Finish() before initializing again.</exception>
    Public Sub Init(HostingApplicationName As String, Optional TableFileName As String = "", Optional GameName As String = "")
        If Pinball Is Nothing Then
            Try
                Dim HostAppFilename = HostingApplicationName.Replace(".", "")

                For Each C As Char In Path.GetInvalidFileNameChars()
                    HostAppFilename = HostAppFilename.Replace("" + C, "")
                Next

                For Each C As Char In Path.GetInvalidPathChars()
                    HostAppFilename = HostAppFilename.Replace("" + C, "")
                Next
                HostAppFilename = "GlobalConfig_{0}".Build(HostAppFilename)
                Dim F As FileInfo

                F = New FileInfo(Path.Combine(New FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName, "config", HostAppFilename + ".xml"))
                If F.Exists = False Then
                    Dim LnkFile As FileInfo
                    LnkFile = New FileInfo(Path.Combine(New FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName, "config", HostAppFilename + ".lnk"))
                    If LnkFile.Exists Then
                        Dim ConfigDirPath As String
                        ConfigDirPath = ResolveShortcut(LnkFile)
                        If Directory.Exists(ConfigDirPath) Then
                            F = New FileInfo(Path.Combine(ConfigDirPath, HostAppFilename + ".xml"))
                        End If
                    End If
                End If




                Pinball = New DirectOutput.Pinball()

                Pinball.Setup(F.FullName, TableFileName, GameName)
                Pinball.Init()
            Catch E As Exception

                Log.Warning(String.Format("DirectOutputComObject: A exception occured while initializing DOF: {0}", E.Message))
                Log.Exception(E)
                Throw (New Exception(String.Format("DirectOutputComObject: A exception occured while initializing DOF: {0}", E.Message)))
            End Try

        Else

            Throw (New Exception("Object has already been initialized. Call Finish() before initializing again."))
        End If

    End Sub

    Private Function ResolveShortcut(ShortcutFile As FileInfo) As String
        Dim TargetPath As String = ""

        Try
            Dim WScriptShell As Type = Type.GetTypeFromProgID("WScript.Shell")
            Dim Shell As Object = Activator.CreateInstance(WScriptShell)
            Dim Para As Object = {ShortcutFile.FullName}
            Dim Shortcut As Object = WScriptShell.InvokeMember("CreateShortcut", BindingFlags.InvokeMethod, Nothing, Shell, Para)
            TargetPath = Shortcut.GetType().InvokeMember("TargetPath", BindingFlags.GetProperty, Nothing, Shortcut, Nothing)
            Shortcut = Nothing
            Shell = Nothing
        Catch ex As Exception

        End Try

        Try

            If Directory.Exists(TargetPath) Then
                Return TargetPath
            ElseIf File.Exists(TargetPath) Then
                Return TargetPath
            Else
                Return ""
            End If
        Catch ex As Exception
            Return ""
        End Try


    End Function




    ''' <summary>
    ''' Shows the frontend of the DirectOutput framework.
    ''' </summary>
    ''' <exception cref="System.Exception">Init has to be called before the frontend is opend.</exception>
    Public Sub ShowFrontend()

        If Pinball IsNot Nothing Then
            Try
                DirectOutput.Frontend.MainMenu.Open(Pinball)
            Catch ex As Exception
                System.Windows.Forms.MessageBox.Show("Could not show DirectOutput frontend.\n The following exception occured:\n{0}".Build(ex.Message), "DirectOutput")
            End Try
        Else
            Throw New Exception("Init has to be called before the frontend is opend.")
        End If

    End Sub


    ''' <summary>
    ''' Gets the descriptors for configured table elments.
    ''' </summary>
    ''' <returns>Array of tabler element descriptors</returns>
    Public Function GetConfiguredTableElmentDescriptors() As String()
        If Pinball IsNot Nothing Then
            Return Pinball.Table.TableElements.GetTableElementDescriptors()
        End If
        Return {}
    End Function

    ''' <summary>
    ''' Return the Name and Path of the TableMapping file.
    ''' </summary>
    ''' <returns>Name and Path of the TableMapping file.</returns>
    Public Function TableMappingFileName() As String
        Try
            If Pinball IsNot Nothing Then
                If (Pinball.GlobalConfig IsNot Nothing) Then
                    Dim FI As FileInfo
                    FI = Pinball.GlobalConfig.GetTableMappingFile()
                    If (FI IsNot Nothing) Then
                        Return FI.FullName
                    End If
                End If
            End If
            Return ""
        Catch E As Exception
            Log.Warning("DirectOutputComObject: A exception occured while getting the TableMappingFilename")
            Log.Exception(E)
            Throw New Exception("DirectOutputComObject: A exception occured while getting the TableMappingFilename", E)
        End Try
    End Function



    Private Property Pinball As Pinball



#Region "IDisposable Support"
    Private disposedValue As Boolean ' So ermitteln Sie überflüssige Aufrufe

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: Verwalteten Zustand löschen (verwaltete Objekte).

            End If
            Finish()

            ' TODO: Nicht verwaltete Ressourcen (nicht verwaltete Objekte) freigeben und Finalize() unten überschreiben.
            ' TODO: Große Felder auf NULL festlegen.
        End If
        Me.disposedValue = True
    End Sub


    ' Dieser Code wird von Visual Basic hinzugefügt, um das Dispose-Muster richtig zu implementieren.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Ändern Sie diesen Code nicht. Fügen Sie oben in Dispose(ByVal disposing As Boolean) Bereinigungscode ein.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class


