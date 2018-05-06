Imports System.Data.SqlClient

Public Class BaseExceptionsHandler
    Protected mHasException As Boolean
    ''' <summary>
    ''' Indicate the last operation thrown an 
    ''' exception or not
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property HasException() As Boolean
        Get
            Return mHasException
        End Get
    End Property
    Protected mLastException As Exception
    ''' <summary>
    ''' Provides access to the last exception thrown
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property LastException() As Exception
        Get
            Return mLastException
        End Get
    End Property
    Public ReadOnly Property SqlException() As SqlException
        Get
            Return CType(mLastException, SqlException)
        End Get
    End Property
    ''' <summary>
    ''' Indicates if there was a sql related exception
    ''' </summary>
    Public ReadOnly Property HasSqlException() As Boolean
        Get
            If LastException IsNot Nothing Then
                Return TypeOf LastException Is SqlException
            Else
                Return False
            End If
        End Get
    End Property
    ''' <summary>
    ''' If you don't need the entire exception as in 
    ''' LastException this provides just the text of the exception
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property LastExceptionMessage() As String
        Get
            Return LastException.Message
        End Get
    End Property
    ''' <summary>
    ''' Indicate for return of a function if there was an 
    ''' exception thrown or not.
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property IsSuccessFul() As Boolean
        Get
            Return Not HasException
        End Get
    End Property
    ''' <summary>
    ''' Returns an array of the entire exception list in reverse order
    ''' (innermost to outermost exception)
    ''' </summary>
    ''' <param name="ex">The original exception to work off</param>
    ''' <returns>Array of Exceptions from innermost to outermost</returns>
    Public Function InnerExceptions(ex As Exception) As Exception()
        Dim exceptions As New List(Of Exception)()
        exceptions.Add(ex)

        Dim currentEx As Exception = ex
        Do While currentEx.InnerException IsNot Nothing
            exceptions.Add(currentEx)
        Loop

        ' Reverse the order to the innermost is first
        exceptions.Reverse()

        Return exceptions.ToArray()

    End Function
End Class
