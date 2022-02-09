<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="SITConnect.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link href="//maxcdn.bootstrapcdn.com/font-awesome/4.1.0/css/font-awesome.min.css" rel="stylesheet">

    <script src="https://www.google.com/recaptcha/api.js?render=6LeCOkUeAAAAABrPjsjeHv5pw3fa9GKTI2V3x920"></script>
    <script>
        grecaptcha.ready(function () {
            grecaptcha.execute('6LeCOkUeAAAAABrPjsjeHv5pw3fa9GKTI2V3x920', { action: 'contact_us' }).then(function (token) {
               document.getElementById("<%=hf_token.ClientID%>").value = token;
            });
        });
    </script>


    <script>



        function Showalert(msg) {
            $("#loginError").show();
            $("#loginError").html("<span>Invalid Email or Password</span>");
            // alert("Your Email or Password is incorrect. Please try again.");
            //window.location.href = "/Login";
        }
        function ShowAccountMsg() {
            $("#loginError").show();
            $("#loginError").html("<span>Your Account is temporarily blocked for 4 minutes due to consective wrong attempts. Please contact admin. </span>");
        }
        const validateEmail = (email) => {
            return String(email)
                .toLowerCase()
                .match(
                    /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/
                );
        };


        function ValidateForm() {
            var error = false;
            $("#loginError").hide();
            if ($("input[id*='txtEmail']").val() == "") {
                error = true;
                $("#emailError").text("Please enter email address.");
            }
            else if (!validateEmail($("input[id*='txtEmail']").val().trim())) {
                error = true;
                $("#emailError").text("Email format is not correct.");
            }
            else
                $("#emailError").text("");

            if ($("input[id*='txtPassword']").val() == "") {
                error = true;
                $("#passError").text("Please enter password.");
            }
            else
                $("#passError").text("");

            if (error)
                return false;
        }
        function ShowalertRecaptcha() {
            $("#loginError").show();
            $("#loginError").html("<span>Invalid Captcha</span>");
        }
    </script>
    <style>
        grecaptcha-badge{
            position:relative !important;

        }
        	.input_field {
		position: relative;
		margin-bottom: 20px;
        -webkit-animation: bounce 0.6s ease-out;
  	     animation: bounce 0.6s ease-out;
		>span {
			position: absolute;
			left: 0;
			top: 0;
			color: #333;
			height: 100%;
			border-right: 1px solid $grey;
			text-align: center;
			width: 30px;
			>i {
				padding-top: 10px;
			}
		}
	}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-2 jumbotron" style="box-shadow: 5px 5px 55px 8px; padding: 60px">
        <h3 class="text-center">Login Here</h3>
        <div class="alert alert-danger" style="display: none" id="loginError">
           
        </div>


        <div class="form-group">
            <span><i aria-hidden="true" class="fa fa-envelope"></i></span>
            <label>Email address:</label>
            
            <asp:TextBox ID="txtEmail" runat="server" class="form-control txtEmail" onKeyup="ValidateForm()"></asp:TextBox>
            <span style="color: red" id="emailError"></span>
            <asp:RegularExpressionValidator ValidationExpression="^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$" ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEmail"  ErrorMessage="Email formate is not correct." style="color:red"></asp:RegularExpressionValidator>

        </div>
        <div class="form-group">
                       <span><i aria-hidden="true" class="fa fa-key"></i></span>
            <label for="pwd">Password:</label>
            <asp:TextBox ID="txtPassword" TextMode="Password" runat="server" class="form-control txtPassword" onKeyup="ValidateForm()"></asp:TextBox>
            <span style="color: red" id="passError"></span>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="txtPassword"  runat="server" ErrorMessage="Please Enter password" style="color:red"></asp:RequiredFieldValidator>

        </div>
        <div class="form-group row">
            <div class="col-12">
                <input type="hidden" id="hf_token" name="hf_token"  runat="server"/>
            </div>
        
        </div>

        <asp:Button ID="btnLogin" class="btn btn-primary" runat="server" Text="Login" OnClick="btnLogin_Click" OnClientClick="return ValidateForm()" />

    </div>
</asp:Content>
