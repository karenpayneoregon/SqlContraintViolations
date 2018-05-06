Public Class Form1
    Private bs As New BindingSource
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Dim row As DataRow = CType(bs.Current, DataRowView).Row
        Dim ops As New DataOperations

        If ops.Update(row.Field(Of String)("FirstName"), row.Field(Of String)("LastName"), row.Field(Of Integer)("id")) Then
            MessageBox.Show("Update successful")
        Else
            If ops.HasException Then
                If ops.SqlException.Message.Contains("unique index") Then
                    row.RejectChanges()
                    Dim fieldPluralize As String = "field"

                    If ops.ConstraintColumns.Contains(",") Then
                        fieldPluralize &= "s"
                    End If

                    MessageBox.Show($"The change to {fieldPluralize} {ops.ConstraintColumns} values '{ops.ConstraintValue}' violates a constraint, reverting back to pre-edit value")

                Else
                    MessageBox.Show("Update failed, contact support.")
                End If
            End If
        End If
    End Sub
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim ops As New DataOperations
        bs.DataSource = ops.Read
        DataGridView1.DataSource = bs
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim row As DataRow = CType(bs.Current, DataRowView).Row
        row.SetField(Of String)("FirstName", "Karen")
        row.SetField(Of String)("LastName", "Payne")
    End Sub
End Class
