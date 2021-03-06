﻿Imports System.IO
Imports PdfSharp
Imports PdfSharp.Drawing
Imports PdfSharp.Pdf
Imports PdfSharp.Drawing.Layout

Public Class frmMain
    Dim cDataAccess As New clsDataAccess

    Private Sub mnuFileImport_Click(sender As Object, e As EventArgs) Handles mnuFileImport.Click
        Dim frmImport As New frmImport
        frmImport.MdiParent = Me
        frmImport.Show()

    End Sub

    Private Sub mnuEditMembers_Click(sender As Object, e As EventArgs) Handles mnuEditMembers.Click
        Dim frmMembers As New frmMembers
        frmMembers.MdiParent = Me
        frmMembers.Show()
    End Sub

    Private Sub mnuViewMembersSNPs_Click(sender As Object, e As EventArgs) Handles mnuViewMembersSNPs.Click
        Dim frmMembersSNPs As New frmMembersSNPs
        frmMembersSNPs.MdiParent = Me
        frmMembersSNPs.Show()
    End Sub

    Private Sub mnuViewTree_Click(sender As Object, e As EventArgs) Handles mnuViewTree.Click
        Dim frmTree As New frmTree
        frmTree.MdiParent = Me
        frmTree.Show()
    End Sub

    Private Sub mnuReportsMembers_Click(sender As Object, e As EventArgs) Handles mnuReportsMembers.Click
        Call ReportMemberReport()
    End Sub

    Public Function ReportMemberReport()
        Dim ds As DataSet
        Dim i As Integer = 0
        Dim intColumn1 As Integer = 100
        Dim intColumn2 As Integer = 150
        Dim intColumn3 As Integer = 250
        Dim intColumn4 As Integer = 350
        Dim intColumn5 As Integer = 450
        Dim intColumn6 As Integer = 550
        Dim intCurrentRow As Integer = 100
        Dim intSpacer As Integer = 15
        Dim strMemberName As String = ""

        Dim document As PdfDocument = New PdfDocument
        document.Info.Title = "Member Page"

        Call modPDF.CoverPage(document, "Member Page", "Members in the database")

        ' Create an empty page
        Dim pdfpage As PdfPage = document.AddPage

        ' Get an XGraphics object for drawing
        Dim graph As XGraphics = XGraphics.FromPdfPage(PdfPage)
        Dim image As PdfSharp.Drawing.XImage = PdfSharp.Drawing.XImage.FromFile(IMAGE_DIRECTORY & "DNAPic2-Large.bmp")
        image.Interpolate = False

        'Make the page border
        Call modPDF.PageBorder(pdfpage, "Portrait", graph)

        'Get the data
        ds = cDataAccess.GetMembersAll
        If ds.Tables(0).Rows.Count > 0 Then
            '  graph.DrawString("Missing SNPs From Tree", font12_bold, brush_black, intColumn1, 480)
            graph.DrawString("Total: " & ds.Tables(0).Rows.Count, font10_bold, black_brush, intColumn5, 75)
            For i = 0 To ds.Tables(0).Rows.Count - 1
                If ds.Tables(0).Rows(i).IsNull("MemberName") = False Then
                    strMemberName = ds.Tables(0).Rows(i).Item("MemberName")
                End If

                If i < 20 Then
                    graph.DrawString(strMemberName, font8, black_brush, intColumn1, intCurrentRow)
                End If

                If i = 20 Then
                    intCurrentRow = 100
                End If

                If i >= 20 Then
                    graph.DrawString(strMemberName, font8, black_brush, intColumn3, intCurrentRow)

                End If

                intCurrentRow = intCurrentRow + intSpacer
            Next
        End If

        'Make sure the directory exists
        Call CreateDirectory(REPORT_DIRECTORY)

        ' Save the document...
        Dim filename As String = REPORT_DIRECTORY & "MembersReport.pdf"
        document.Save(filename)

        ' ...and start a viewer.
        Process.Start(filename)
    End Function


End Class