<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="SITConnect.Register" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
      <link href="//maxcdn.bootstrapcdn.com/font-awesome/4.1.0/css/font-awesome.min.css" rel="stylesheet">
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
    <div class="container mt-2 jumbotron" style="box-shadow: 5px 5px 55px 8px; padding: 60px">
   <h3 class="text-center">Register Here</h3>
        <div class="alert alert-danger" style="display:none" id="loginError">
          <asp:Label ID="Message" runat="server" Text="Label" style="color:red" ></asp:Label>
        </div>
       
        <div class="form-group">
                   <span><i aria-hidden="true" class="fa fa-user"></i></span>
            <label>First Name </label>
           <asp:TextBox ID="txtFName" runat="server" class="form-control" ></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtFName" ErrorMessage="Please Enter First Name" style="color:red"></asp:RequiredFieldValidator>
        </div>
        <div class="form-group">
                   <span><i aria-hidden="true" class="fa fa-user"></i></span>
            <label>Last Name:</label>
        <asp:TextBox ID="txtLName" runat="server" class="form-control" ></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtLName" ErrorMessage="Please Enter Last Name" style="color:red"></asp:RequiredFieldValidator>

        </div>
        <div class="form-group">
                   <span><i aria-hidden="true" class="fa fa-credit-card"></i></span>
            <label>Credit Card Info</label>
            <asp:TextBox ID="txtCredCard" TextMode="Password" runat="server" class="form-control" ></asp:TextBox>

        </div>
        <div class="form-group">
                   <span><i aria-hidden="true" class="fa fa-envelope"></i></span>
            <label>Email address:</label>
            <asp:TextBox ID="txtEmail" TextMode="Email" runat="server" class="form-control" ></asp:TextBox>
            <asp:RegularExpressionValidator ValidationExpression="^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$" ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEmail"  ErrorMessage="Email formate is not correct." style="color:red"></asp:RegularExpressionValidator>
        </div>
        <div class="form-group">
                   <span><i aria-hidden="true" class="fa fa-calendar"></i></span>
             <label>Date of Birth:</label>
               <asp:TextBox ID="txtDOB" runat="server" TextMode="Date" class="form-control" ></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="txtDOB"  runat="server" ErrorMessage="Please Enter your date of birth" style="color:red"></asp:RequiredFieldValidator>
        </div>
        <div class="form-group float-left" style="width:45%">
                   <span><i aria-hidden="true" class="fa fa-key"></i></span>
             <label for="pwd">Password:</label>
          <asp:TextBox ID="txtPassword" TextMode="Password" runat="server" class="form-control" onKeyup="validatePassword();" ></asp:TextBox>
            <span id="passError" style="color:red"></span><br />
            <asp:CustomValidator ID="CustomValidator1" runat="server"  ErrorMessage="CustomValidator" OnServerValidate="CustomValidator1_ServerValidate" style="color:red"></asp:CustomValidator>
            
          
        </div>
        <div class="float-right" style="width:40%">
            <h6 class="">Password Instructions</h6>
            <ul>
                <li>Password must be minimum 12 characters. </li>
                <li>
                    Use combination of lower-case, upper-case, Numbers and special characters.
                </li>
            </ul>
        </div>
        <div class="clearfix"></div>
        <div class="form-group">
            <asp:FileUpload ID="FileUpload1" runat="server" AllowMultiple="false" accept=".png,.jpg,.jpeg,.gif" />
            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="FileUpload1" ErrorMessage="Please select an image" style="color:red"></asp:RequiredFieldValidator>

        </div>
        <asp:Button ID="Button1" class="btn btn-primary" runat="server" Text="Signup" OnClick="Button1_Click" OnClientClick="return Validate()" />
        &nbsp;<br />
        <asp:Label ID="lb_error1" runat="server"></asp:Label>
        <br />

</div>
   
</asp:Content>

