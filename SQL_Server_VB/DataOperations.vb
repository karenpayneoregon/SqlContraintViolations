Imports System.Data.SqlClient
Imports System.Text.RegularExpressions

Public Class DataOperations
    Inherits BaseSqlServerConnections

    ''' <summary>
    ''' Column(s) violating constraint
    ''' </summary>
    Public ConstraintColumns As String = Nothing
    ''' <summary>
    ''' Column values violating a constraint
    ''' </summary>
    Public ConstraintValue As String = Nothing

    Public Sub New()
        DefaultCatalog = "ForumExample"
    End Sub
    Public Function Read() As DataTable
        Dim dt As New DataTable

        Dim selectStatement = "SELECT id,FirstName,LastName FROM dbo.Persons1 ORDER BY LastName"

        Using cn As New SqlConnection() With {.ConnectionString = ConnectionString}
            Using cmd As New SqlCommand() With {.Connection = cn, .CommandText = selectStatement}

                cn.Open()
                dt.Load(cmd.ExecuteReader)

            End Using
        End Using

        Return dt

    End Function
    Public Function Update(pFirstName As String, pLastName As String, pIdentifier As Integer) As Boolean

        Using cn As New SqlConnection() With {.ConnectionString = ConnectionString}

            Dim statement = "UPDATE dbo.Persons1 SET FirstName = @FirstName,LastName = @LastName  WHERE id = @Id"

            Using cmd As New SqlCommand() With {.Connection = cn, .CommandText = statement}

                cmd.Parameters.AddWithValue("@FirstName", pFirstName)
                cmd.Parameters.AddWithValue("@LastName", pLastName)
                cmd.Parameters.AddWithValue("@id", pIdentifier)

                Try

                    cn.Open()
                    cmd.ExecuteNonQuery()

                    Return True

                Catch ex As SqlException

                    Dim message As String
                    Dim pos As Integer

                    '
                    ' Proposed values for update causing the exception
                    '
                    ConstraintValue = Regex.Match(ex.Message, "\(([^)]*)\)").Groups(1).Value

                    pos = ex.Message.IndexOf(".", StringComparison.Ordinal)
                    message = ex.Message.Substring(0, pos)

                    If message.Contains("Cannot insert duplicate key row in object") Then
                        ConstraintColumns = GetPerson1IndexKeys(cmd, ex.Message)
                    End If

                    mHasException = True
                    mLastException = ex

                    Return False

                Catch ex As Exception

                    mHasException = True
                    mLastException = ex

                    Return False
                End Try
            End Using
        End Using
    End Function
    ''' <summary>
    ''' Get indexes for table Person1 along with their
    ''' index keys
    ''' </summary>
    ''' <param name="cmd"></param>
    ''' <param name="message"></param>
    ''' <returns></returns>
    Public Function GetPerson1IndexKeys(cmd As SqlCommand, message As String) As String

        Dim indexName = "Unknown"

        cmd.CommandText = "EXEC sp_helpindex N'dbo.Persons1'"

        Try

            Dim reader = cmd.ExecuteReader()
            If reader.HasRows Then
                While reader.Read()
                    If message.Contains(reader.GetString(0)) Then
                        indexName = reader.GetString(2)
                    End If
                End While
            End If

        Catch ex As Exception
            '
            ' Should not land here but must be careful as 
            ' this method is called from a catch block
            '
        End Try

        Return indexName

    End Function

End Class