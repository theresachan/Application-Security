<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="SITConnect.ChangePassword" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script>
        function ShowMsg() {

        }
    </script>
    <script>

        function Showalert(msg) {
            alert("You Registered with SITConnect. Please open your email acccount to verify your account.");
            window.location.href = "/Login";
        }

        function validatePassword() {
            $("#ContentPlaceHolder1_CustomValidator1").text("");
            var password = $("input[id*='txtPassword']").val();

            passwordChanged(password);
            // var p = document.getElementById('newPassword').value,
            errors = [];
            if (password.length < 12) {
                $("#passError").text("Your password must be at least 12 characters.");
                return false;
            }
            else if (password.search(/[a-z]/i) < 0) {
                $("#passError").text("Your password must contain at least one letter.");
                return false;
            }
            else if (password.test(/[0-9]/) < 0) {
                $("#passError").text("Your password must contain at least one digit.");
                return false;
            }
            else {
                ("#passError").text("");
                return true;
            }
        }

        function passwordChanged(password) {
            var strength = document.getElementById('passError');
            var strongRegex = new RegExp("^(?=.{14,})(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9])(?=.*\\W).*$", "g");
            var mediumRegex = new RegExp("^(?=.{10,})(((?=.*[A-Z])(?=.*[a-z]))|((?=.*[A-Z])(?=.*[0-9]))|((?=.*[a-z])(?=.*[0-9]))).*$", "g");
            var enoughRegex = new RegExp("(?=.{12,}).*", "g");
            var pwd = password;
            if (pwd.length == 0) {
                strength.innerHTML = 'Type Password';
            } else if (false == enoughRegex.test(pwd)) {
                strength.innerHTML = 'More Characters';
            } else if (strongRegex.test(pwd)) {
                strength.innerHTML = '<span style="color:green">Strong!</span>';
            } else if (mediumRegex.test(pwd)) {
                strength.innerHTML = '<span style="color:orange">Medium!</span>';
            } else {
                strength.innerHTML = '<span style="color:red">Weak!</span>';
            }
        }
        function Validate() {

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
      <div class="container mt-2" style="box-shadow: 5px 5px 55px 8px; padding: 60px">
   <h3 class="text-center">Change Password Here</h3>
        <div class="alert alert-danger" style="display:none" id="loginError">
          <asp:Label ID="Message" runat="server" Text="Label" style="color:red" ></asp:Label>
        </div>
<div class="row">
     <div class="form-group " style="width:45%">
             <label for="pwd">Enter Current Password:</label>
          <asp:TextBox ID="txtOldPassword" TextMode="Password" runat="server" class="form-control"  ></asp:TextBox>
        </div>
</div>
 <div class="row">
     
        <div class="form-group float-left" style="width:45%">
             <label for="pwd">Enter New Password:</label>
          <asp:TextBox ID="txtPassword" TextMode="Password" runat="server" class="form-control" onKeyup="validatePassword();" ></asp:TextBox>
            <span id="passError" style="color:red"></span><br />
            <asp:CustomValidator ID="CustomValidator1" runat="server" ControlToValidate="txtPassword"  ErrorMessage="CustomValidator" OnServerValidate="CustomValidator1_ServerValidate" style="color:red"></asp:CustomValidator>
            
          
        </div>
        <div class="float-right" style="width:50%;margin-left:36px">
            <h6 class="">Password Instructions</h6>
            <ul>
                <li>Password must be minimum 12 characters. </li>
                <li>
                    Use combination of lower-case, upper-case, Numbers and special characters.
                </li>
            </ul>
        </div>
 </div>

        <div class="clearfix"></div>
      
        <asp:Button ID="Button1" class="btn btn-primary" runat="server" Text="Change Password" OnClick="Button1_Click" OnClientClick="return Validate()" />

</div>
</asp:Content>
