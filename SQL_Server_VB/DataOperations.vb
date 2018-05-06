Imports System.Data.SqlClient
Imports System.Text.RegularExpressions

Public Class DataOperations
    Inherits BaseSqlServerConnections

    ''' <summary>
    ''' Column(s) violating constraint
    ''' </summary>
    Public ConstraintColumns As String = Nothing
    Public DuplicateCountryValue As String = Nothing
    ''' <summary>
    ''' Column values violating a constraint
    ''' </summary>
    Public ConstraintValue As String = Nothing

    Public Sub New()
        DefaultCatalog = "ForumExample"
    End Sub
    ''' <summary>
    ''' Read all Persons rows
    ''' </summary>
    ''' <returns></returns>
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
    ''' <summary>
    ''' Update a person without causing an exception againsts a constraint but
    ''' will increment the primary key without adding a new record
    ''' </summary>
    ''' <param name="pFirstName">First name to update</param>
    ''' <param name="pLastName">Last name to update</param>
    ''' <param name="pIdentifier">Identifying key for person</param>
    ''' <returns></returns>
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
                        ConstraintColumns = GetIndexKeys(cmd, ex.Message, "Persons1")
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
    ''' Update person the right way by first determing if FirstName and LastName
    ''' will not produce a duplicate record or increment the next primary key sequence.
    ''' </summary>
    ''' <param name="pFirstName">First name to update</param>
    ''' <param name="pLastName">Last name to update</param>
    ''' <param name="pIdentifier">Identifying key for person</param>
    ''' <returns></returns>
    Public Function Update1(pFirstName As String, pLastName As String, pIdentifier As Integer) As Boolean

        Using cn As New SqlConnection() With {.ConnectionString = ConnectionString}

            Dim statement = "SELECT 1 FROM  dbo.Persons1 AS p WHERE  p.FirstName = @FirstName AND p.LastName = @LastName "

            Using cmd As New SqlCommand() With {.Connection = cn, .CommandText = statement}

                cmd.Parameters.AddWithValue("@FirstName", pFirstName)
                cmd.Parameters.AddWithValue("@LastName", pLastName)

                Try

                    cn.Open()

                    If cmd.ExecuteScalar() Is Nothing Then
                        cmd.Parameters.AddWithValue("@id", pIdentifier)
                        cmd.ExecuteNonQuery()
                        Return True
                    Else
                        Return False
                    End If

                Catch ex As SqlException

                    '
                    ' Proposed values for update causing the exception
                    '
                    ConstraintValue = Regex.Match(ex.Message, "\(([^)]*)\)").Groups(1).Value


                    If ex.Number = 2601 Then
                        ConstraintColumns = GetIndexKeys(cmd, ex.Message, "Persons1")
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
    Public Property ConstraintColumnName() As String
    ''' <summary>
    ''' This method shows how to assert against adding a duplicate country. 
    ''' There are several parts of this method that are overkill e.g. using
    ''' regx to get the value to be inserted, the table name and the index
    ''' which was violated for educational purposes if you were to get into
    ''' this topic in a generic manner
    ''' </summary>
    ''' <param name="pCountryName"></param>
    ''' <param name="pIdentifier"></param>
    ''' <param name="pError"></param>
    ''' <returns></returns>
    Public Function InsertCountry(ByVal pCountryName As String, ByRef pIdentifier As Integer, ByRef pError As String) As Boolean
        Using cn = New SqlConnection() With {.ConnectionString = ConnectionString}
            Using cmd = New SqlCommand() With {.Connection = cn}
                Dim insertStatement = "INSERT INTO dbo.Country (Name)  VALUES (@Name);" & "SELECT CAST(scope_identity() AS int);"

                Try
                    cmd.CommandText = insertStatement
                    cmd.Parameters.AddWithValue("@Name", pCountryName)

                    cn.Open()

                    pIdentifier = Convert.ToInt32(cmd.ExecuteScalar())
                    Return True
                Catch ex As SqlException
                    Dim message As String = Nothing
                    Dim tableName As String = ""
                    Dim indexName As String = ""

                    '                        
                    ' * We already know the value but if you want to get
                    ' * into some regx this shows how to parse the value.
                    '                         
                    DuplicateCountryValue = Regex.Match(ex.Message, "\(([^)]*)\)").Groups(1).Value

                    '                        
                    ' * Get the table name 'country' which we have in the INSERT INTO
                    '                         
                    Dim match = Regex.Match(ex.Message, "'([^']*)")
                    If match.Success Then
                        tableName = match.Groups(1).Value
                    End If


                    If ex.Number = 2601 Then

                        pError = $"Can not add '{DuplicateCountryValue}' into '{tableName}' since it already exists."
                        ' if you needed the index involved with the error
                        indexName = GetIndexKeys(cmd, ex.Message, "Country")
                    End If

                    mHasException = True
                    mLastException = ex

                    Return False
                End Try
            End Using
        End Using
    End Function
    ''' <summary>
    ''' Insert new record the right way by first determing if the country name
    ''' is present in the database and will not increment the primary key sequence.
    ''' </summary>
    ''' <param name="pCountryName">Country name to insert</param>
    ''' <param name="pIdentifier">New primary key</param>
    ''' <param name="pError">Error message on failure</param>
    ''' <returns>Success</returns>
    Public Function InsertCountry1(ByVal pCountryName As String, ByRef pIdentifier As Integer, ByRef pError As String) As Boolean
        Using cn = New SqlConnection() With {.ConnectionString = ConnectionString}
            Using cmd = New SqlCommand() With {.Connection = cn}
                Dim selectStatement = "SELECT 1 FROM ForumExample.dbo.Country WHERE Name = @Name"

                Dim insertStatement = "INSERT INTO dbo.Country (Name)  VALUES (@Name);" & "SELECT CAST(scope_identity() AS int);"

                Try
                    cmd.CommandText = selectStatement
                    cmd.Parameters.AddWithValue("@Name", pCountryName)
                    cn.Open()

                    If cmd.ExecuteScalar() IsNot Nothing Then
                        pError = $"Country '{pCountryName}' already in table"
                        mHasException = False
                        Return False
                    End If

                    cmd.CommandText = insertStatement


                    pIdentifier = Convert.ToInt32(cmd.ExecuteScalar())

                    Return True

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
    Public Function GetIndexKeys(ByVal cmd As SqlCommand, ByVal message As String, ByVal tableName As String) As String

        Dim indexName = "Unknown"

        cmd.CommandText = $"EXEC sp_helpindex N'dbo.{tableName}'"
        Try

            Dim reader = cmd.ExecuteReader()
            If reader.HasRows Then
                Do While reader.Read()
                    If message.Contains(reader.GetString(0)) Then
                        indexName = reader.GetString(2)
                    End If
                Loop
            End If

        Catch e1 As Exception
            '
            ' Should not land here but must be careful as 
            ' this method is called from a catch block.
            ' Now if you do land here the table name does
            ' not exists or you spelled the name wrong.
            '
        End Try

        Return indexName

    End Function

End Class