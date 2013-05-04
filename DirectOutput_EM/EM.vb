Imports System.Runtime.InteropServices
Imports System.IO

''' <summary>
''' Com object class allowing access to the DirectOutput framework functions for EM tables which are not using PinMame or B2S.Server.
''' This object can be instanciated from VBS using the following line of code:
''' 
''' ~~~~~~~~~~~~~{.vbs}
'''   Set ObjectVariable=CreateObject("DirectOutput.EM")
''' ~~~~~~~~~~~~~
''' 
''' After the object has been instaciated, the DirectOutput framework has to be intialized using the command below. The RomName parameter is optional, but it is recommanded to populate it with some unique shortname for the EM table. The DiredctOutput framework will use the value of this para, to lookup the table config. 
''' 
''' ~~~~~~~~~~~~~{.vbs}
'''   ObjectVariable.Initialize "PathAndFilenameOfTheTable","FakeRomName"
''' ~~~~~~~~~~~~~
'''
''' After the framework has been intialized you can start to send the states of your table elements as follows (Please read the docu on SetTableElementState for details):
''' 
''' ~~~~~~~~~~~~~{.vbs}
'''   ObjectVariable.SetTableElementState "S",5,20
''' ~~~~~~~~~~~~~
''' 
''' 
'''</summary>
<ProgId("DirectOutput.EM"), ComClass(EM.ClassId, EM.InterfaceId, EM.EventsId)> _
Public Class EM
    Implements IDisposable

#Region "COM-GUIDs"
    Public Const ClassId As String = "46405fc8-ce13-4436-a218-d5b6bbae0da0"
    Public Const InterfaceId As String = "6ce5481d-3948-4cf5-bd48-c396b7c8f7ec"
    Public Const EventsId As String = "ae2cbc14-18cf-4229-b597-12257d2b99f4"
#End Region


    Private Pinball As DirectOutput.Pinball

    Private IsInitialized As Boolean = False

    ''' <summary>
    ''' Initializes the DirectOutput framework for the specified TableFile or Romname.<br />
    ''' Since EM tables dont use Roms, the Romname para kann be populated by some kind of identifier for the table. The Romnamepara is used to lookup configs in LedCOntrol.ini files (yes, you can configure toys for EM tables through LedControl.ini).
    ''' </summary>
    ''' <param name="TableFileName">Name of the table file.</param>
    ''' <param name="Romname">Fake Romname as explained above (optional).</param>
    ''' <exception cref="System.Exception">
    ''' Could not initialize the DirectOutput framework for table {0} and rom name {1}.
    ''' or
    ''' DirectOutput has already been initialized.
    ''' </exception>
    Public Sub Initialize(TableFileName As String, Optional Romname As String = "")
        If Not IsInitialized Then
            Try
                'Pinball.Init(New FileInfo(TableFileName), Romname)
            Catch ex As Exception
                Throw New Exception(String.Format("Could not initialize the DirectOutput framework for table {0} and rom name {1}.", TableFileName, Romname), ex)
            End Try
            IsInitialized = True
            IsFinished = False
        Else
            Throw New Exception("DirectOutput has already been initialized.")
        End If

    End Sub

    Private IsFinished As Boolean = False

    ''' <summary>
    ''' Finishes the DirectOutput framework.<br />
    ''' This method has to be called before a table is shut down.
    ''' </summary>
    ''' <exception cref="System.Exception">A exception occured when finishing the Directoutput framework.</exception>
    Public Sub Finish()
        Try
            Pinball.Finish()
        Catch ex As Exception
            Throw New Exception("A exception occured when finishing the Directoutput framework.", ex)
        End Try
    End Sub


    ''' <summary>
    ''' Sets the value of a table element.<br />
    ''' Use this method to let the DirectOutput framework know about the values of the table elements on you EM table. DirectOutput will translate the table element values into effects for the toys in your cabinet, the same way as for SS tables using Pinmame. This means you can use all config options (own XML configs and LedContril.ini files) available in the DirectOutput framework.
    ''' </summary>
    ''' <param name="TableElementTypeChar">Char representing the type of the table element.<br />
    ''' Valid types are:
    ''' - S = solenoid
    ''' - L = lamp
    ''' - W = switch
    ''' - G = general illumination
    ''' - M = mech
    ''' Other types of table elements might exist in your version of the framework. Please read the docu on TableElementTypeEnum for a complete list of supported types.</param>
    ''' <param name="Number">The number of the table element.</param>
    ''' <param name="Value">The value of the table element.</param>
    ''' <exception cref="System.Exception">
    ''' Could not set the value of table element {0} {1} to {2}.
    ''' or
    ''' DirectOutput has to be initialized before setting table element values.
    ''' </exception>
    Public Sub SetTableElementValue(TableElementTypeChar As Char, Number As Integer, Value As Integer)
        If IsInitialized Then

            Try
                Pinball.ReceiveData(TableElementTypeChar, Number, Value)
            Catch Ex As Exception
                Throw New Exception(String.Format("Could not set the value of table element {0} {1} to {2}.", TableElementTypeChar, Number, Value), Ex)
            End Try

        Else
            Throw New Exception("DirectOutput has to be initialized before setting table element values.")
        End If


    End Sub

#Region "Finalize & Dispose"
    ''' <summary>
    ''' Disposes the DirectOutput.EM object.
    ''' </summary>
    Public Overloads Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

    Protected Overridable Overloads Sub Dispose(ByVal Disposing As Boolean)
        If Disposing Then

            ' Free other state (managed objects).
        End If
        ' Free your own state (unmanaged objects).
        ' Set large fields to null.

        If Not IsFinished And IsInitialized Then
            Try
                Pinball.Finish()
            Catch
            End Try
            IsFinished = True
            IsInitialized = False
        End If
    End Sub

    Protected Overrides Sub Finalize()
        ' Simply call Dispose(False).
        Dispose(False)
    End Sub

#End Region

    ''' <summary>
    ''' Initializes a new instance of the <see cref="EM"/> class.
    ''' </summary>
    Public Sub New()
        MyBase.New()

        Pinball = New DirectOutput.Pinball()

    End Sub

End Class


