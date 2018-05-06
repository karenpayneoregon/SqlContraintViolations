<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.cmdSetupForFailure = New System.Windows.Forms.Button()
        Me.cmdUpdateCurrent = New System.Windows.Forms.Button()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.Panel1.SuspendLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.cmdSetupForFailure)
        Me.Panel1.Controls.Add(Me.cmdUpdateCurrent)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel1.Location = New System.Drawing.Point(0, 334)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(411, 49)
        Me.Panel1.TabIndex = 0
        '
        'cmdSetupForFailure
        '
        Me.cmdSetupForFailure.Location = New System.Drawing.Point(143, 11)
        Me.cmdSetupForFailure.Name = "cmdSetupForFailure"
        Me.cmdSetupForFailure.Size = New System.Drawing.Size(123, 23)
        Me.cmdSetupForFailure.TabIndex = 1
        Me.cmdSetupForFailure.Text = "Set for violation"
        Me.cmdSetupForFailure.UseVisualStyleBackColor = True
        '
        'cmdUpdateCurrent
        '
        Me.cmdUpdateCurrent.Location = New System.Drawing.Point(13, 11)
        Me.cmdUpdateCurrent.Name = "cmdUpdateCurrent"
        Me.cmdUpdateCurrent.Size = New System.Drawing.Size(124, 23)
        Me.cmdUpdateCurrent.TabIndex = 0
        Me.cmdUpdateCurrent.Text = "Update current"
        Me.cmdUpdateCurrent.UseVisualStyleBackColor = True
        '
        'DataGridView1
        '
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.DataGridView1.Location = New System.Drawing.Point(0, 0)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.Size = New System.Drawing.Size(411, 334)
        Me.DataGridView1.TabIndex = 1
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(411, 383)
        Me.Controls.Add(Me.DataGridView1)
        Me.Controls.Add(Me.Panel1)
        Me.Name = "Form1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Code sample"
        Me.Panel1.ResumeLayout(False)
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Panel1 As Panel
    Friend WithEvents cmdUpdateCurrent As Button
    Friend WithEvents DataGridView1 As DataGridView
    Friend WithEvents cmdSetupForFailure As Button
End Class
