Imports System.Data.OleDb
Public Class clsDataAccess
    Public Function GetAllMembers() As DataSet
        Dim sql As String

        sql = "  SELECT * FROM  tblMembers "
        Try
            Dim dbCommandAccess As OleDb.OleDbCommand = New OleDbCommand
            dbCommandAccess.CommandText = sql
            dbCommandAccess.CommandType = CommandType.Text

            '  If conn.State = ConnectionState.Closed Then
            dbCommandAccess.Connection = GetConnection()
            dbCommandAccess.Connection.Open()
            ' End If

            Dim dataAdapter As OleDb.OleDbDataAdapter = New OleDbDataAdapter
            dataAdapter.SelectCommand = dbCommandAccess
            Dim dataSet As System.Data.DataSet = New System.Data.DataSet()
            dataAdapter.Fill(dataSet)

            dbCommandAccess.Connection.Close()

            Return dataSet
        Catch ex As Exception
            MsgBox("Error:" & ex.Message)
        End Try

    End Function

    Public Function GetMemberByID(ByVal mintID As Integer) As DataSet
        Dim sql As String

        sql = "  SELECT * FROM  tblMembers WHERE ID= " & mintID
        Try
            Dim dbCommandAccess As OleDb.OleDbCommand = New OleDbCommand
            dbCommandAccess.CommandText = sql
            dbCommandAccess.CommandType = CommandType.Text

            '  If conn.State = ConnectionState.Closed Then
            dbCommandAccess.Connection = GetConnection()
            dbCommandAccess.Connection.Open()
            ' End If

            Dim dataAdapter As OleDb.OleDbDataAdapter = New OleDbDataAdapter
            dataAdapter.SelectCommand = dbCommandAccess
            Dim dataSet As System.Data.DataSet = New System.Data.DataSet()
            dataAdapter.Fill(dataSet)

            dbCommandAccess.Connection.Close()

            Return dataSet
        Catch ex As Exception
            MsgBox("Error:" & ex.Message)
        End Try

    End Function


    Public Function GetMembersAll() As DataSet
        Dim sql As String

        sql = "  SELECT * FROM  tblMembers ORDER BY MemberName "
        Try
            Dim dbCommandAccess As OleDb.OleDbCommand = New OleDbCommand
            dbCommandAccess.CommandText = sql
            dbCommandAccess.CommandType = CommandType.Text

            '  If conn.State = ConnectionState.Closed Then
            dbCommandAccess.Connection = GetConnection()
            dbCommandAccess.Connection.Open()
            ' End If

            Dim dataAdapter As OleDb.OleDbDataAdapter = New OleDbDataAdapter
            dataAdapter.SelectCommand = dbCommandAccess
            Dim dataSet As System.Data.DataSet = New System.Data.DataSet()
            dataAdapter.Fill(dataSet)

            dbCommandAccess.Connection.Close()

            Return dataSet
        Catch ex As Exception
            MsgBox("Error:" & ex.Message)
        End Try

    End Function

    Public Function GetMemberByFTDNAID(ByVal vstrFTDNAID As String) As DataSet
        Dim sql As String

        sql = "  SELECT * FROM  tblMembers WHERE FTDNAID =" & Chr(34) & vstrFTDNAID & Chr(34)
        Try
            Dim dbCommandAccess As OleDb.OleDbCommand = New OleDbCommand
            dbCommandAccess.CommandText = sql
            dbCommandAccess.CommandType = CommandType.Text

            '  If conn.State = ConnectionState.Closed Then
            dbCommandAccess.Connection = GetConnection()
            dbCommandAccess.Connection.Open()
            ' End If

            Dim dataAdapter As OleDb.OleDbDataAdapter = New OleDbDataAdapter
            dataAdapter.SelectCommand = dbCommandAccess
            Dim dataSet As System.Data.DataSet = New System.Data.DataSet()
            dataAdapter.Fill(dataSet)

            dbCommandAccess.Connection.Close()

            Return dataSet
        Catch ex As Exception
            MsgBox("Error:" & ex.Message)
        End Try
    End Function

    Public Function GetPositionsByMemberID(ByVal vintMemberID As Integer) As DataSet
        Dim sql As String

        sql = "  SELECT * FROM  tblMemberPositions WHERE FK_MemberID =" & vintMemberID
        Try
            Dim dbCommandAccess As OleDb.OleDbCommand = New OleDbCommand
            dbCommandAccess.CommandText = sql
            dbCommandAccess.CommandType = CommandType.Text

            '  If conn.State = ConnectionState.Closed Then
            dbCommandAccess.Connection = GetConnection()
            dbCommandAccess.Connection.Open()
            ' End If

            Dim dataAdapter As OleDb.OleDbDataAdapter = New OleDbDataAdapter
            dataAdapter.SelectCommand = dbCommandAccess
            Dim dataSet As System.Data.DataSet = New System.Data.DataSet()
            dataAdapter.Fill(dataSet)

            dbCommandAccess.Connection.Close()

            Return dataSet
        Catch ex As Exception
            MsgBox("Error:" & ex.Message)
        End Try
    End Function

    Public Function GetSNPByPositionRefAlt(ByVal vintPosition As Integer, _
                                           ByVal vstrRef As String, _
                                           ByVal vstrAlt As String) As DataSet
        Dim sql As String

        sql = " SELECT tblMemberPositions.ID,MemberName,Pos,Ref,Alt,Qual,Filter FROM tblMemberPositions, tblMembers WHERE tblMemberPositions.FK_MemberID = tblMembers.ID "
        sql = sql & " AND Pos=" & vintPosition
        sql = sql & " AND Ref=" & Chr(34) & vstrRef & Chr(34)
        sql = sql & " AND Alt=" & Chr(34) & vstrAlt & Chr(34)

        Try
            Dim dbCommandAccess As OleDb.OleDbCommand = New OleDbCommand
            dbCommandAccess.CommandText = sql
            dbCommandAccess.CommandType = CommandType.Text

            '  If conn.State = ConnectionState.Closed Then
            dbCommandAccess.Connection = GetConnection()
            dbCommandAccess.Connection.Open()
            ' End If

            Dim dataAdapter As OleDb.OleDbDataAdapter = New OleDbDataAdapter
            dataAdapter.SelectCommand = dbCommandAccess
            Dim dataSet As System.Data.DataSet = New System.Data.DataSet()
            dataAdapter.Fill(dataSet)

            dbCommandAccess.Connection.Close()

            Return dataSet
        Catch ex As Exception
            MsgBox("Error:" & ex.Message)
        End Try
    End Function






    Public Function GetMemberByYFullID(ByVal YFullID As String) As DataSet
        Dim sql As String

        sql = "  SELECT * FROM  tblMembers WHERE YFullID =" & Chr(34) & YFullID & Chr(34)
        Try
            Dim dbCommandAccess As OleDb.OleDbCommand = New OleDbCommand
            dbCommandAccess.CommandText = sql
            dbCommandAccess.CommandType = CommandType.Text

            '  If conn.State = ConnectionState.Closed Then
            dbCommandAccess.Connection = GetConnection()
            dbCommandAccess.Connection.Open()
            ' End If

            Dim dataAdapter As OleDb.OleDbDataAdapter = New OleDbDataAdapter
            dataAdapter.SelectCommand = dbCommandAccess
            Dim dataSet As System.Data.DataSet = New System.Data.DataSet()
            dataAdapter.Fill(dataSet)

            dbCommandAccess.Connection.Close()

            Return dataSet
        Catch ex As Exception
            MsgBox("Error:" & ex.Message)
        End Try

    End Function

    Public Function InsertMember(ByVal vstrMemberName As String,
                                 ByVal vstrFTDNAID As String, _
                                 ByVal vstrYFullID As String) As Integer
        Dim sql As String
        Dim rowsAffected As Integer
        Try

            sql = "  INSERT INTO tblMembers (MemberName, FTDNAID, YFullID) VALUES ("
            sql = sql & Chr(34) & vstrMemberName & Chr(34)
            sql = sql & "," & Chr(34) & vstrFTDNAID & Chr(34)
            sql = sql & "," & Chr(34) & vstrYFullID & Chr(34)
            sql = sql & ")"

            Dim dbCommandAccess As OleDb.OleDbCommand = New OleDbCommand
            dbCommandAccess.CommandText = sql
            dbCommandAccess.CommandType = CommandType.Text

            '  If conn.State = ConnectionState.Closed Then
            dbCommandAccess.Connection = GetConnectionDocFiles()
            dbCommandAccess.Connection.Open()
            ' End If

            rowsAffected = dbCommandAccess.ExecuteNonQuery()
            ' End If


            dbCommandAccess.Connection.Close()

            Return rowsAffected
        Catch ex As Exception
            MsgBox("Error:" & ex.Message)
        End Try
    End Function

    Public Function InsertPositionByMemberID(ByVal vstrFK_MemberID As Integer,
                                             ByVal vintPosition As Integer, _
                                             ByVal strRef As String, _
                                             ByVal vstrAlt As String, _
                                             ByVal vdblQual As Double, _
                                             ByVal vstrFilter As String, _
                                             ByVal vstrInfo As String, _
                                             ByVal vstrFormat As String, _
                                             ByVal vstrMutation As String
                                             ) As Integer

        Dim sql As String
        Dim rowsAffected As Integer
        Try

            sql = "  INSERT INTO tblMemberPositions (FK_MemberID, Pos, Ref, Alt, Qual, Filter, Info, Format, Mutation) VALUES ("

            sql = sql & vstrFK_MemberID
            sql = sql & "," & vintPosition
            sql = sql & "," & Chr(34) & strRef & Chr(34)
            sql = sql & "," & Chr(34) & vstrAlt & Chr(34)
            sql = sql & "," & vdblQual
            sql = sql & "," & Chr(34) & vstrFilter & Chr(34)
            sql = sql & "," & Chr(34) & vstrInfo & Chr(34)
            sql = sql & "," & Chr(34) & vstrFormat & Chr(34)
            sql = sql & "," & Chr(34) & vstrMutation & Chr(34)
            sql = sql & ")"

            Dim dbCommandAccess As OleDb.OleDbCommand = New OleDbCommand
            dbCommandAccess.CommandText = sql
            dbCommandAccess.CommandType = CommandType.Text

            If conn.State = ConnectionState.Closed Then
                dbCommandAccess.Connection = GetConnection()
                dbCommandAccess.Connection.Open()
                rowsAffected = dbCommandAccess.ExecuteNonQuery()
            Else
                dbCommandAccess.Connection = conn
                rowsAffected = dbCommandAccess.ExecuteNonQuery()
            End If





            '     dbCommandAccess.Connection.Close()

            Return rowsAffected
        Catch ex As Exception
            MsgBox("Error:" & ex.Message)
        End Try
    End Function

    Public Function UpdateMember(ByVal vstrMemberName As String,
                                 ByVal vstrFTDNAID As String, _
                                 ByVal vstrYFullID As String, _
                                 ByVal vintID As Integer) As Integer
        Dim sql As String
        Dim rowsAffected As Integer
        Try

            sql = "  UPDATE tblMembers "
            sql = sql & " SET MemberName= " & Chr(34) & vstrMemberName & Chr(34)
            sql = sql & ",FTDNAID=" & Chr(34) & vstrFTDNAID & Chr(34)
            sql = sql & ",YFullID=" & Chr(34) & vstrYFullID & Chr(34)
            sql = sql & " WHERE ID=" & vintID

            Dim dbCommandAccess As OleDb.OleDbCommand = New OleDbCommand
            dbCommandAccess.CommandText = sql
            dbCommandAccess.CommandType = CommandType.Text

            '  If conn.State = ConnectionState.Closed Then
            dbCommandAccess.Connection = GetConnectionDocFiles()
            dbCommandAccess.Connection.Open()
            ' End If

            rowsAffected = dbCommandAccess.ExecuteNonQuery()
            ' End If


            dbCommandAccess.Connection.Close()

            Return rowsAffected
        Catch ex As Exception
            MsgBox("Error:" & ex.Message)
        End Try
    End Function
End Class
