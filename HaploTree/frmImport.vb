﻿Imports System.IO
Imports System.IO.StreamReader

Imports System.Data.OleDb
Imports System.Text

Public Class frmImport
    Dim cDataAccess As New clsDataAccess
    Dim mstrFileName As String = ""
    Dim mintMemberID As Integer = 0
    Dim mProcess As Process


    Private Sub btnBrowse_Click(sender As Object, e As EventArgs) Handles btnBrowse.Click
        'Declare the variables
        Dim strLine As String
        Dim strChrom As String = ""
        Dim strPosition As String = ""
        Dim strID As String = ""
        Dim strRef As String = ""
        Dim strAlt As String = ""
        Dim strQual As String = ""
        Dim strFilter As String = ""
        Dim strInfo As String = ""
        Dim strFormat As String = ""
        Dim strMutation As String = ""
        Dim strHold As String = ""
        Dim i As Integer = 0

        'For checking the speed of the import
        Dim dtmStart As DateTime '
        Dim dtmEnd As DateTime
        OpenFileDialog1.InitialDirectory = App_Path()
        OpenFileDialog1.Filter = "vcf files (*vcf)|*.vcf|All files (*.*)|*.*"
        OpenFileDialog1.FilterIndex = 2
        OpenFileDialog1.RestoreDirectory = True
        OpenFileDialog1.FileName = "variants"

        If OpenFileDialog1.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
            lblPathAndFileName.Text = OpenFileDialog1.FileName
            'Set the filename
            mstrFileName = OpenFileDialog1.FileName

            'Add the column headers for the listview
            Me.lvwImport.Columns.Add("Position", 80, HorizontalAlignment.Left)
            lvwImport.Columns.Add("Ref", 50, HorizontalAlignment.Left)
            lvwImport.Columns.Add("Alt", 50, HorizontalAlignment.Left)
            lvwImport.Columns.Add("Qual", 70, HorizontalAlignment.Left)
            lvwImport.Columns.Add("Filter", 60, HorizontalAlignment.Left)
            lvwImport.Columns.Add("Info", 140, HorizontalAlignment.Left)
            lvwImport.Columns.Add("Format", 140, HorizontalAlignment.Left)
            lvwImport.Columns.Add("Mutation", 140, HorizontalAlignment.Left)

            Dim LineItemsArr() As String

            'Start the timer
            dtmStart = Now
            Me.Cursor = Cursors.WaitCursor

            Dim strFile As New FileStream(OpenFileDialog1.FileName, FileMode.Open)
            Dim strReader As New StreamReader(strFile)

            'Read first line.
            strLine = Replace(strReader.ReadLine, Chr(9), "", 1, -1)

            'Write to log file
            GenFunc.WriteLogEntry(dtmStart.ToLongTimeString & " Start import")
            Dim intGV As Integer = 0

            'Loop over each line in file, While list is Not Nothing.
            Do While (Not strLine Is Nothing)
                If strLine.Substring(0, 4) = "##co" Then
                    intGV = strLine.IndexOf("hg19")
                    If intGV > 0 Then
                        lblGenomeVersion.Text = "HG 19"
                    Else
                        intGV = strLine.IndexOf("hg38")
                        If intGV > 0 Then
                            lblGenomeVersion.Text = "HG 38"
                        Else
                            lblGenomeVersion.Text = "Unknown"
                        End If
                    End If

                End If

                'lblGenomeVersion
                If strLine.Substring(0, 4) = "chrY" Then 'If we do not run into the ChrY then we need to keep skipping lines.
                    Application.DoEvents()

                    'Parses the line into an array.
                    LineItemsArr = strLine.Split(Chr(9))

                    'Assigns the the array as variables
                    strChrom = Replace(LineItemsArr(0).ToString.Trim.PadLeft(8, "0"), Chr(34), "", 1, -1)
                    strPosition = Replace(LineItemsArr(1).ToString.Trim, Chr(34), "", 1, -1)
                    strRef = Replace(LineItemsArr(3).ToString.Trim, Chr(34), "", 1, -1)
                    strAlt = Replace(LineItemsArr(4).ToString.Trim, Chr(34), "", 1, -1)
                    strQual = Replace(LineItemsArr(5).ToString.Trim, Chr(34), "", 1, -1)
                    strFilter = Replace(LineItemsArr(6).ToString.Trim, Chr(34), "", 1, -1)
                    strInfo = Replace(LineItemsArr(7).ToString.Trim, Chr(34), "", 1, -1)
                    strFormat = Replace(LineItemsArr(8).ToString.Trim, Chr(34), "", 1, -1)
                    strMutation = Replace(LineItemsArr(9).ToString.Trim, Chr(34), "", 1, -1)

                    'Assembles the variables that we want into an array
                    Dim str(9) As String
                    str(0) = strPosition
                    str(1) = strRef
                    str(2) = strAlt
                    str(3) = strQual
                    str(4) = strFilter
                    str(5) = strInfo
                    str(6) = strFormat
                    str(7) = strMutation

                    'This is the logic that tells us whether or not we will be adding the row to the database OR rejecting it.
                    If strFilter = "PASS" Then
                        'The alt and ref MUST be diferent
                        If strAlt <> strRef Then
                            If strMutation.Substring(0, 3) = "1/1" Then
                                'This is a good record and will be added to the database
                                Dim itm As New ListViewItem(str)
                                lvwImport.Items.Add(itm)
                            Else

                            End If
                        Else 'The alt and the ref are the same
                        End If
                    End If

                    'reset the variables
                    strChrom = ""
                    strPosition = ""
                    strID = ""
                    strRef = ""
                    strAlt = ""
                    strQual = ""
                    strFilter = ""
                    strInfo = ""
                    strFormat = ""
                    strMutation = ""

                    ' Read in the next line.
                    strLine = strReader.ReadLine
                Else
                    strLine = strReader.ReadLine
                End If
            Loop
            Me.Cursor = Cursors.Arrow

            strFile.Close()

            lblPassingPositions.Text = lvwImport.Items.Count
            dtmEnd = Now

            'Write to log file
            GenFunc.WriteLogEntry(dtmEnd.ToLongTimeString & " End import")

            MsgBox("Import Complete!", MsgBoxStyle.Information, "IMPORT COMPLETE")


        End If
    End Sub

    Private Sub btnRead_Click(sender As Object, e As EventArgs)
        'Declare the variables
        Dim strLine As String
        Dim strChrom As String = ""
        Dim strPosition As String = ""
        Dim strID As String = ""
        Dim strRef As String = ""
        Dim strAlt As String = ""
        Dim strQual As String = ""
        Dim strFilter As String = ""
        Dim strInfo As String = ""
        Dim strFormat As String = ""
        Dim strMutation As String = ""
        Dim strHold As String = ""
        Dim i As Integer = 0

        'For checking the speed of the import
        Dim dtmStart As DateTime '
        Dim dtmEnd As DateTime


        'Add the column headers for the listview
        Me.lvwImport.Columns.Add("Position", 80, HorizontalAlignment.Left)
        lvwImport.Columns.Add("Ref", 50, HorizontalAlignment.Left)
        lvwImport.Columns.Add("Alt", 50, HorizontalAlignment.Left)
        lvwImport.Columns.Add("Qual", 70, HorizontalAlignment.Left)
        lvwImport.Columns.Add("Filter", 60, HorizontalAlignment.Left)
        lvwImport.Columns.Add("Info", 140, HorizontalAlignment.Left)
        lvwImport.Columns.Add("Format", 140, HorizontalAlignment.Left)
        lvwImport.Columns.Add("Mutation", 140, HorizontalAlignment.Left)

        ''Add the column headers for the listview
        'Me.lvwRejected.Columns.Add("Position", 100, HorizontalAlignment.Left)
        'lvwRejected.Columns.Add("Ref", 50, HorizontalAlignment.Left)
        'lvwRejected.Columns.Add("Alt", 50, HorizontalAlignment.Left)
        'lvwRejected.Columns.Add("Qual", 50, HorizontalAlignment.Left)
        'lvwRejected.Columns.Add("Filter", 50, HorizontalAlignment.Left)
        'lvwRejected.Columns.Add("Info", 120, HorizontalAlignment.Left)
        'lvwRejected.Columns.Add("Format", 120, HorizontalAlignment.Left)
        'lvwRejected.Columns.Add("Mutation", 120, HorizontalAlignment.Left)
        'lvwRejected.Columns.Add("Reason", 120, HorizontalAlignment.Left)

        '#CHROM	POS	ID	REF	ALT	QUAL	FILTER	INFO	FORMAT
        Dim LineItemsArr() As String

        '   If OpenFileDialog1.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
        'Start the timer
        dtmStart = Now
        Me.Cursor = Cursors.WaitCursor

        Dim strFile As New FileStream(OpenFileDialog1.FileName, FileMode.Open)
        Dim strReader As New StreamReader(strFile)



        'Read first line.
        strLine = Replace(strReader.ReadLine, Chr(9), "", 1, -1)

        'Write to log file
        GenFunc.WriteLogEntry(dtmStart.ToLongTimeString & " Start import")
        Dim intGV As Integer = 0

        'Loop over each line in file, While list is Not Nothing.
        Do While (Not strLine Is Nothing)
            If strLine.Substring(0, 4) = "##co" Then
                intGV = strLine.IndexOf("hg19")
                If intGV > 0 Then
                    lblGenomeVersion.Text = "HG 19"
                Else
                    intGV = strLine.IndexOf("hg38")
                    If intGV > 0 Then
                        lblGenomeVersion.Text = "HG 38"
                    Else
                        lblGenomeVersion.Text = "Unknown"
                    End If
                End If

            End If

            'lblGenomeVersion
            If strLine.Substring(0, 4) = "chrY" Then 'If we do not run into the ChrY then we need to keep skipping lines.
                Application.DoEvents()

                'Parses the line into an array.
                LineItemsArr = strLine.Split(Chr(9))

                'Assigns the the array as variables
                strChrom = Replace(LineItemsArr(0).ToString.Trim.PadLeft(8, "0"), Chr(34), "", 1, -1)
                strPosition = Replace(LineItemsArr(1).ToString.Trim, Chr(34), "", 1, -1)
                strRef = Replace(LineItemsArr(3).ToString.Trim, Chr(34), "", 1, -1)
                strAlt = Replace(LineItemsArr(4).ToString.Trim, Chr(34), "", 1, -1)
                strQual = Replace(LineItemsArr(5).ToString.Trim, Chr(34), "", 1, -1)
                strFilter = Replace(LineItemsArr(6).ToString.Trim, Chr(34), "", 1, -1)
                strInfo = Replace(LineItemsArr(7).ToString.Trim, Chr(34), "", 1, -1)
                strFormat = Replace(LineItemsArr(8).ToString.Trim, Chr(34), "", 1, -1)
                strMutation = Replace(LineItemsArr(9).ToString.Trim, Chr(34), "", 1, -1)

                'Assembles the variables that we want into an array
                Dim str(9) As String
                str(0) = strPosition
                str(1) = strRef
                str(2) = strAlt
                str(3) = strQual
                str(4) = strFilter
                str(5) = strInfo
                str(6) = strFormat
                str(7) = strMutation

                'This is the logic that tells us whether or not we will be adding the row to the database OR rejecting it.
                If strFilter = "PASS" Then
                    'The alt and ref MUST be diferent
                    If strAlt <> strRef Then
                        If strMutation.Substring(0, 3) = "1/1" Then
                            'This is a good record and will be added to the database
                            Dim itm As New ListViewItem(str)
                            lvwImport.Items.Add(itm)
                        Else
                            'This is a PASS BUT the read is not "1/1"
                            'str(8) = "This is a 'PASS' BUT column 'J' it is NOT 1/1."   'This is the reason it is not being added.

                            ''Add this to the rejected even though it is a pass
                            'Dim itmReject As New ListViewItem(str)
                            'itmReject = New ListViewItem(str)
                            'Me.lvwRejected.Items.Add(itmReject)

                        End If
                    Else 'The alt and the ref are the same
                        'str(8) = "This is a 'PASS' BUT the ALT and REF are the same. "
                        'Dim itmReject As New ListViewItem(str)
                        'Me.lvwRejected.Items.Add(itmReject)
                        'strHold = strHold & strLine & vbCrLf
                    End If
                End If

                'This colors the individual cells - Not needed at the moment.
                'Select Case strRef
                '    Case "T"
                '        lvwImport.Items(i).UseItemStyleForSubItems = False
                '        lvwImport.Items(i).SubItems(1).BackColor = Color.Red
                '        lvwImport.Items(i).SubItems(1).ForeColor = Color.White
                '    Case "A"
                '        lvwImport.Items(i).UseItemStyleForSubItems = False
                '        lvwImport.Items(i).SubItems(1).BackColor = Color.Green
                '        lvwImport.Items(i).SubItems(1).ForeColor = Color.White
                '    Case "G"
                '        lvwImport.Items(i).UseItemStyleForSubItems = False
                '        lvwImport.Items(i).SubItems(1).BackColor = Color.Orange
                '        lvwImport.Items(i).SubItems(1).ForeColor = Color.White
                '    Case "C"
                '        lvwImport.Items(i).UseItemStyleForSubItems = False
                '        lvwImport.Items(i).SubItems(1).BackColor = Color.DarkBlue
                '        lvwImport.Items(i).SubItems(1).ForeColor = Color.White
                'End Select



                'Select Case strAlt
                '    Case "T"
                '        lvwImport.Items(i).UseItemStyleForSubItems = False
                '        lvwImport.Items(i).SubItems(2).BackColor = Color.Red
                '        lvwImport.Items(i).SubItems(2).ForeColor = Color.White
                '    Case "A"
                '        lvwImport.Items(i).UseItemStyleForSubItems = False
                '        lvwImport.Items(i).SubItems(2).BackColor = Color.Green
                '        lvwImport.Items(i).SubItems(2).ForeColor = Color.White
                '    Case "G"
                '        lvwImport.Items(i).UseItemStyleForSubItems = False
                '        lvwImport.Items(i).SubItems(2).BackColor = Color.Orange
                '        lvwImport.Items(i).SubItems(2).ForeColor = Color.White
                '    Case "C"
                '        lvwImport.Items(i).UseItemStyleForSubItems = False
                '        lvwImport.Items(i).SubItems(2).BackColor = Color.DarkBlue
                '        lvwImport.Items(i).SubItems(2).ForeColor = Color.White
                'End Select



                '    'Else
                '    '    Dim itm As New ListViewItem(str)
                '    '    lvwRejected.Items.Add(itm)
                '    'End If





                '    '"A"s are green, "G"s are orange, "C"s are blue and "T"s are red

                '    'If strAlt = "T" Then
                '    '    ListView2.Items(i).UseItemStyleForSubItems = False
                '    '    ListView2.Items(i).SubItems(2).BackColor = Color.Green
                '    'End If

                '    i = i + 1

                '    '   Call test(strChrom, strPosition, strID, strRef, strAlt, strQual, strFilter, strInfo, strFormat)
                '    '  intReturn = cDataAccess.InsFileImport(strTechID, strLastName, strFirstName, strOccuranceDate, intStore, intTerminal, intTransaction,
                '    '  strItem, strItemDescription, dblExtPrice, dblTax, dblFA, intEvent)

                '    'Track the returns how many successful vs how many failed inserts
                '    ' If intReturn = -1 Then   'Error
                '    'Call WriteLog("ERROR: modGateway.ReadFile: Failed Insert on the FileImport: " & strTechID & "-" & strFirstName & " " & strLastName) 'Need to report where it blew up
                '    ' End If

                '      End If
                'reset the variables
                strChrom = ""
                strPosition = ""
                strID = ""
                strRef = ""
                strAlt = ""
                strQual = ""
                strFilter = ""
                strInfo = ""
                strFormat = ""
                strMutation = ""

                ' Read in the next line.
                strLine = strReader.ReadLine
            Else
                strLine = strReader.ReadLine
            End If
        Loop
        Me.Cursor = Cursors.Arrow

        strFile.Close()

        lblPassingPositions.Text = lvwImport.Items.Count
        dtmEnd = Now

        'Write to log file
        GenFunc.WriteLogEntry(dtmEnd.ToLongTimeString & " End import")

        '   End If
    End Sub

   




    Private Sub lvwImport_ColumnClick(sender As Object, e As ColumnClickEventArgs)
        'Set the ListViewItemSorter property to a new ListViewItemComparer object.
        Me.lvwImport.ListViewItemSorter = New ListViewItemComparer(e.Column)
        ' Call the sort method to manually sort.
        lvwImport.Sort()
    End Sub

   


    Private Sub Button1_Click(sender As Object, e As EventArgs)
        ' Dim file As Byte() = CType(oleAccReader("DocFile"), Byte())
        '   Dim file As Byte() = CType(oleAccReader("DocFile"), Byte())
        '                    Using ms As New IO.MemoryStream(file)



        Dim FILE_NAME As String = "F:\HaploTree\Files\B64384\Darins hg38 BigY\variants.vcf"



        ' Dim fs As Stream = FileUpload1.PostedFile.InputStream
        Dim fs As Stream = New IO.MemoryStream(Encoding.ASCII.GetBytes(FILE_NAME))
        Dim br As New BinaryReader(fs)
        Dim bytes As Byte() = br.ReadBytes(fs.Length)
        Dim rowsaffected As Integer = 0

        'insert the file into database 
        Dim strQuery As String = "Insert Into tblDocFiles (FK_MemberID,DocType,DocFile) Values (@FK_MemberID,@DocType,@File)"
        Dim cmd As New OleDbCommand(strQuery)
        cmd.Parameters.Add("@FK_MemberID", SqlDbType.VarChar).Value = 1
        cmd.Parameters.Add("@DocType", SqlDbType.VarChar).Value() = "My File Type"
        cmd.Parameters.Add("@File", SqlDbType.Binary).Value = bytes


        cmd.Connection = DataConn.GetConnectionDocFiles
        cmd.Connection.Open()
        rowsaffected = cmd.ExecuteNonQuery()
    End Sub

    Private Sub Button2_Click_1(sender As Object, e As EventArgs)
        Call GetData()

    End Sub


    Public Function GetData()
        Dim strAccQuery As String = "select DocFile from tblDocFiles where id=3 "
        '  Dim cn As OleDb.OleDbConnection
        Dim connMS As OleDbConnection
        Try
            connMS = DataConn.GetConnectionDocFiles

            'Using Structure Simplifies Garbage Collection And Ensures That The Object Will Be Disposed Of Correctly Afterwards
            Using connMS
                'Create Command Object, To Make Use Of SQL Query
                Dim dbCommand As OleDbCommand = New OleDbCommand(strAccQuery, connMS)
                'Open Connection To The Database
                connMS.Open()


                'Reader Object To Traverse Through Found Records
                Dim oleAccReader As OleDbDataReader = dbCommand.ExecuteReader
                If oleAccReader.HasRows Then 'If The Reader Finds Rows
                    While oleAccReader.Read() 'Retrieve The Content, For Each Match
                        ''GetString(1) Represents Column 2 Of StudentsInfo table
                        'txtName.Text = oleAccReader.GetString(1)

                        ''GetString(2) Gets Information Stored In Third Column
                        'txtSurname.Text = oleAccReader.GetString(2)

                        ''Use GetValue or GetInt32 Here, Because StudentNumber Is A Number Field
                        'txtStudentNumber.Text = oleAccReader.GetValue(0)

                        Call WriteData(oleAccReader.GetValue(0))


                        'Dim file As Byte() = CType(oleAccReader("DocFile"), Byte())
                        'Using ms As New IO.MemoryStream(file)
                        '    Dim sr As StreamReader = New StreamReader(ms)
                        '    Dim textSsS As String = sr.ReadToEnd()


                        '    Call WriteData(textSsS)


                        '    '  Me.rtfDoc.Rtf = textSsS

                        'End Using
                    End While
                Else
                    'No Match Was Found
                    MessageBox.Show("No Rows Found.")
                End If
                'Close Reader Object
                oleAccReader.Close()
            End Using
        Catch ex As Exception
            '  Call WriteErrorLog("ERROR: frmPhoto/GetPhoto:" & ex.Message)
        End Try

    End Function

    Public Function WriteData(ByVal vstrdatatowrite As String)
        Dim path As String = "c:\MyTest.txt"

        ' This text is added only once to the file.
        If File.Exists(path) = False Then

            ' Create a file to write to.
            Dim createText As String = vstrdatatowrite '+ Environment.NewLine
            File.WriteAllText(path, createText)
        End If

        ' This text is always added, making the file longer over time
        ' if it is not deleted.
        '   Dim appendText As String = "This is extra text" + Environment.NewLine
        '  File.AppendAllText(path, appendText)

        ' Open the file to read from.
        ' Dim readText As String = File.ReadAllText(path)
        ' Console.WriteLine(readText)

    End Function

    Private Sub Button3_Click(sender As Object, e As EventArgs)
        Dim ms As New MemoryStream

        Dim strText As String = File.ReadAllText("F:\HaploTree\Files\B64384\Darins hg38 BigY\variants.vcf")
        '  Dim strText As String = Me.rtfDoc.Rtf
        Dim enc As New UTF8Encoding
        Dim arryBytData() As Byte = enc.GetBytes(strText)
        ms.Write(arryBytData, 0, arryBytData.Length)
        Dim strAccQuery As String = ""

        'This Query Inserts Our Input Data Into The Table
        strAccQuery = "INSERT INTO tblDocFiles (FK_MemberID,DocType,DocFile) VALUES(@ID,@FileType,@File)"
        ' strAccQuery = "Insert Into tblTest (YFullID,Notes) Values (@YFullID,@File)"

        'START HERE
        Dim rowsAffected As Integer
        Dim dbCommand As OleDbCommand = New OleDbCommand(strAccQuery, conn)
        dbCommand.Parameters.Add("@ID", OleDbType.Double, 10).Value = 23
        dbCommand.Parameters.Add("@FileType", OleDbType.VarChar, 10).Value = "New Type"
        dbCommand.Parameters.Add("@File", OleDbType.VarBinary, arryBytData.Length).Value = arryBytData

        dbCommand.Connection = DataConn.GetConnectionDocFiles
        dbCommand.Connection.Open()
        rowsAffected = dbCommand.ExecuteNonQuery()
        dbCommand.Connection.Close()


    End Sub

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        'Dim frmMembers As New frmMembers
        'frmMembers.ShowDialog()


        Dim frmMembersSearch As New frmMembersSearch
        frmMembersSearch.ShowDialog()

        mintMemberID = frmMembersSearch.ID
        Call PopulateForm()

    End Sub

    Public Function PopulateForm()
        Dim ds As DataSet
        Dim dsPositions As DataSet

        ds = cDataAccess.GetMemberByID(mintMemberID)
        If ds.Tables(0).Rows.Count > 0 Then
            Me.lblID.Text = mintMemberID

            If ds.Tables(0).Rows(0).IsNull("MemberName") = False Then
                Me.lblMemberName.Text = ds.Tables(0).Rows(0).Item("MemberName")
            Else
                Me.lblMemberName.Text = ""
            End If

            If ds.Tables(0).Rows(0).IsNull("FTDNAID") = False Then
                Me.lblFTDNAID.Text = ds.Tables(0).Rows(0).Item("FTDNAID")
            Else
                Me.lblFTDNAID.Text = ""
            End If

            If ds.Tables(0).Rows(0).IsNull("YFullID") = False Then
                Me.lblYFullID.Text = ds.Tables(0).Rows(0).Item("YFullID")
            Else
                Me.lblYFullID.Text = ""
            End If

            Me.btnBrowse.Enabled = True

            'First remove all records from the listview
            Me.lvwImport.Clear()

            'Now see if this person has records stored
            dsPositions = cDataAccess.GetPositionsByMemberID(mintMemberID)
            If dsPositions.Tables(0).Rows.Count > 0 Then
                Call FillListview(dsPositions)

            End If





        End If
    End Function


    Public Function FillListview(ByVal dsPositions As DataSet)
        Dim i As Integer = 0
        Dim lvwColumn As ColumnHeader
        Dim itmListItem As ListViewItem
        Dim shtCntr As Short
        Try

            If dsPositions.Tables(0).Rows.Count > 0 Then
                'Do headers first
                Me.lvwImport.Clear()

                Me.lvwImport.Columns.Add("ID", 0, HorizontalAlignment.Left)
                Me.lvwImport.Columns.Add("Position", 80, HorizontalAlignment.Left)
                lvwImport.Columns.Add("Ref", 50, HorizontalAlignment.Left)
                lvwImport.Columns.Add("Alt", 50, HorizontalAlignment.Left)
                lvwImport.Columns.Add("Qual", 70, HorizontalAlignment.Left)
                lvwImport.Columns.Add("Filter", 60, HorizontalAlignment.Left)
                lvwImport.Columns.Add("Info", 140, HorizontalAlignment.Left)
                lvwImport.Columns.Add("Format", 140, HorizontalAlignment.Left)
                lvwImport.Columns.Add("Mutation", 140, HorizontalAlignment.Left)

                For i = 0 To dsPositions.Tables(0).Rows.Count - 1
                    itmListItem = New ListViewItem()
                    itmListItem.Text = dsPositions.Tables(0).Rows(i).Item(0)

                    For shtCntr = 1 To dsPositions.Tables(0).Columns.Count - 1
                        Select Case dsPositions.Tables(0).Columns.Item(shtCntr).ColumnName()
                            Case "ID" ', "MemberName"
                                If dsPositions.Tables(0).Rows(i).Item(shtCntr) Is System.DBNull.Value = True Then
                                    itmListItem.SubItems.Add("")
                                Else
                                    If dsPositions.Tables(0).Rows(i).IsNull(shtCntr) = False Then
                                        itmListItem.SubItems.Add(dsPositions.Tables(0).Rows(i).Item(shtCntr))
                                    Else
                                        itmListItem.SubItems.Add("")
                                    End If
                                End If
                            Case Else
                                If dsPositions.Tables(0).Rows(i).IsNull(shtCntr) = False Then
                                    itmListItem.SubItems.Add(dsPositions.Tables(0).Rows(i).Item(shtCntr))
                                Else
                                    itmListItem.SubItems.Add("")
                                End If

                                'itmListItem.SubItems.Add("")


                        End Select
                    Next shtCntr
                    Me.lvwImport.Items.Add(itmListItem)
                Next

                lblPassingPositions.Text = lvwImport.Items.Count
                '   Me.lblMembers.Text = "Total Members: " & ds.Tables(0).Rows.Count





            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try




    End Function

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        Call SaveFile()
    End Sub


    Public Function SaveFile()
        Dim sql As String = ""
        Dim intFK_MemberID As Integer = 0
        Dim intPosition As String
        Dim intPos As Integer
        Dim strRef As String
        Dim strAlt As String
        Dim strQual As String
        Dim strFilter As String
        Dim strInfo As String
        Dim strFormat As String = ""
        Dim strMutation As String = 0
        Dim i As Integer
        Dim dtmStart As DateTime
        Dim dtmEnd As DateTime

        Dim intPositionID As Integer = 0
        Dim intYFullID As Integer = 1
        Dim dsPos As DataSet
        Dim sqlPos As String = ""

        Dim dsPosition As DataSet



        'For Each lvi As ListViewItem In ListView1.Items
        '    For c As Integer = 0 To lvi.SubItems.Count - 1
        '        MsgBox(lvi.SubItems(c).Text)
        '    Next
        'Next
        dtmStart = Now.ToLongTimeString

        '  For Each lvi As ListViewItem In ListView2.Items
        '  For c As Integer = 0 To lvi.SubItems.Count - 1



        '16606 position
        'http://www.genetichomeland.com/welcome/dnamarkerindex.asp?snp=P109&chromosome=Y


        intFK_MemberID = Me.lblID.Text.Trim
        For i = 0 To Me.lvwImport.Items.Count - 1
            ' MsgBox(lvi.SubItems(c).Text)

            intPosition = lvwImport.Items(i).SubItems(0).Text
            strRef = lvwImport.Items(i).SubItems(1).Text
            strAlt = lvwImport.Items(i).SubItems(2).Text
            strQual = lvwImport.Items(i).SubItems(3).Text
            strFilter = lvwImport.Items(i).SubItems(4).Text
            strInfo = lvwImport.Items(i).SubItems(5).Text

            strFormat = lvwImport.Items(i).SubItems(6).Text
            strMutation = Mid(lvwImport.Items(i).SubItems(7).Text, 1, 1)


            Call cDataAccess.InsertPositionByMemberID(intFK_MemberID, intPosition, strRef, strAlt, strQual, strFilter, strInfo, strFormat, strMutation)

            'If strMutation = "1" Then
            '    dsPos = cDataAccess.GetIDByPosition(intPosition)
            '    If dsPos.Tables(0).Rows.Count > 0 Then
            '        intPos = dsPos.Tables(0).Rows(0).Item("ID")
            '    Else
            '        'Insert the posistion and grab it right away
            '        Call cDataAccess.InsertPosition(intPosition)

            '        dsPos = cDataAccess.GetIDByPosition(intPosition)
            '        If dsPos.Tables(0).Rows.Count > 0 Then
            '            intPos = dsPos.Tables(0).Rows(0).Item("ID")
            '        End If
            '    End If

            '    If strRef.Trim.Length = 1 And strAlt.Trim.Length = 1 Then
            '        Call cDataAccess.InsertSNP(CInt(Me.lblID.Text), intPos, strRef, strAlt, strFilter)
            '    End If

            'End If

        Next
        dtmEnd = Now.ToLongTimeString

        '    Next

    End Function

 

     
    
End Class
