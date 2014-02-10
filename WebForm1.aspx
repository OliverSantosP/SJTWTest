<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="SJTWTest.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
<link href="/Content/bootstrap.css" rel="stylesheet"/>
    
</head>
<body>
    <form id="form1" runat="server">
    <div>

        <textarea class="FormElement" name="Message" id="Message" cols="40" rows="4"></textarea>

    <input type="file" id="myFile" name="myFile"/>
    <a href="#" onclick="showFileName()">Show Name</a>
        <br />
        <asp:Button ID="Button1" runat="server" Text="Button" OnClick="Post" />
        

    </div>
    </form>
</body>
    <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.10.1/jquery.min.js"></script>
<script type="text/javascript">
    function showFileName() {
        var fil = document.getElementById("myFile");
        alert(fil.value);
    }
</script>
</html>
