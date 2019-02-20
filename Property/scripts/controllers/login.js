angular.module('Property').controller('loginController', ['$scope', '$rootScope', '$http', '$location', '$cookies', '$timeout', 'sweetAlert', 'UserService',
    function ($scope, $rootScope, $http, $location, $cookies, $timeout, sweetAlert, UserService) {
        $rootScope.session = { IsAuth: false, IsAccountSetup :false }; 
        $rootScope.userName = '';
        
        $scope.clear = function () {
            $scope.user = { firstname: '', lastname: '', password : ''};
        };

        $scope.clickLogin = function () {
            if ($scope.loginForm.$valid) {
                $scope.loading = true;
                
                //debugger;
                UserService.loginData($scope.user)
                .success(function (response) {
                    $scope.loading = false;
                    if (response.Status === "success") {
                        $rootScope.session = { IsAuth: true, IsAccountSetup: true, IsAdmin : true }; //Get data from code-behind by hitting some API to maintain when user refresh the page

                        if ($rootScope.session.IsAuth && $rootScope.session.IsAccountSetup) {
                            $location.path('/dashboard');
                        }
                        else {
                            sweetAlert.swal({ title: "Oops!", text: "Authentication problem!! Please try later.", type: "error" });
                        }
                    }       
                    else {
                        //debugger;
                        if (response.Message === "VerifyAccountPending.") {
                            $location.path('/reverifyaccount');
                        }
                        else {
                            sweetAlert.swal({ title: "Oops!", text: response.Message, type: "error" });
                        }
                    }
                })
                .error(function (error) {
                    sweetAlert.swal({ title: "Oops!", text: error, type: "error" });
                    $scope.loading = false;
                });
            }
            else {
                sweetAlert.swal({ title: "Oops!", text: "Please enter valid user name and password.", type: "error" });
            }
            
        };
        $scope.clickResendLink = function () {
            if ($scope.reverifyAccountForm.$valid) {
                $scope.loading = true;
                UserService.reverifyAccountData($scope.user)
                .success(function (result) {
                    $scope.loading = false;
                    if (result.Status === "success") {
                        if (result.Message === 1) {
                            $scope.user.email = '';
                            sweetAlert.swal({
                                title: "Yeah",
                                text: "Check your email for instructions for how to verify your account.",
                                type: "success",
                                showCancelButton: false,
                                confirmButtonColor: "#DD6B55",
                                confirmButtonText: "Yes, go ahead!",
                                cancelButtonText: "No, cancel plx!",
                                closeOnConfirm: true,
                                closeOnCancel: false
                            },
                            function (isConfirm) {
                                $location.path('/login');
                            });
                        }
                        else if (result.Message === 2) {
                            $scope.user.email = '';
                            sweetAlert.swal({
                                title: "Yeah",
                                text: "Account is already verified.",
                                type: "warning",
                                showCancelButton: false,
                                confirmButtonColor: "#DD6B55",
                                confirmButtonText: "Yes, go ahead!",
                                cancelButtonText: "No, cancel plx!",
                                closeOnConfirm: true,
                                closeOnCancel: false
                            },
                            function (isConfirm) {
                                $location.path('/login');
                            });
                        }
                        else {
                            sweetAlert.swal({ title: "Oops!", text: "Email is not send successfully.", type: "error" });
                        }
                    }
                    else {
                        sweetAlert.swal({ title: "Oops!", text: result.Message, type: "error" });
                    }
                })
                .error(function (error) {
                    sweetAlert.swal({ title: "Oops!", text: "Something went wrong.Please try later.", type: "error" });
                });
            }
        };
        $scope.clickForgot = function () {
            if ($scope.forgotForm.$valid) {
                $scope.loading = true;
                UserService.forgotData($scope.user)
                .success(function (result) {
                    $scope.loading = false;
                    if (result.Status === "success") {
                        if (result.Message === 1) {
                            $scope.user.email = '';
                            sweetAlert.swal({ title: "Yeah!", text: "Check your email for instructions on how to reset your password.", type: "success" });
                        }
                        else {
                            sweetAlert.swal({ title: "Oops!", text: "Email is not send successfully.", type: "error" });
                        }
                    }
                    else {
                        sweetAlert.swal({ title: "Oops!", text: result.Message, type: "error" });
                    }
                })
                .error(function (error) {
                    sweetAlert.swal({ title: "Oops!", text: "Something went wrong.Please try later.", type: "error" });
                });
            }
        };
        $scope.resetPassword = function () {
            if ($scope.resetForm.$valid) {
                //debugger;
                var requestId = document.getElementById("hfRequestId").value;
                var userId = document.getElementById("hfUserId").value;
                $scope.user.UserId = userId;
                $scope.user.RequestId = requestId;
                $scope.loading = true;
                UserService.ResetPassword($scope.user)
                .success(function (result) {
                    $scope.loading = false;
                    if (result.Status === "success") {
                        sweetAlert.swal({
                            title: "Yeah",
                            text: "Password reset successfully.",
                            type: "success",
                            showCancelButton: false,
                            confirmButtonColor: "#DD6B55",
                            confirmButtonText: "Yes, go ahead!",
                            cancelButtonText: "No, cancel plx!",
                            closeOnConfirm: true,
                            closeOnCancel: false
                        },
                        function (isConfirm) {
                            $location.path('/login');
                        });
                    }
                    else {
                        sweetAlert.swal({ title: "Oops!", text: result.Message, type: "error" });
                    }
                })
                .error(function (err) {
                    $scope.loading = false;
                    sweetAlert.swal({ title: "Oops!", text: "Something went wrong.Please try later.", type: "error" });
                });
            }
        }
        $scope.clickLogout = function () {
            $rootScope.session = { IsAuth: false, IsAccountSetup: false };
            $scope.loading = true;
            $scope.logoutModel = {}; 
            UserService.Logout($scope.logoutModel)
                    .success(function (result) {
                        $scope.loading = false;
                        if (result.Status === "success") {
                            $cookies.remove("productCurrentPage");
                            $location.path('/login');
                        }
                    })
                    .error(function (user) {
                        $scope.loading = false;
                        $.gritter.add({ title: 'Oops!', text: result.Message });
                    });
        };

    }]);
angular.module('Property').factory('UserService', ['$http', function ($http) {
    var UserService = {};
    UserService.saveUserdata = function (registerModel) {
        return $http({ url: '/Account/Register', method: 'POST', contentType: 'application/json; charset=utf-8', data: JSON.stringify(registerModel), DataType: 'Json' })
    }
    UserService.loginData = function (registerModel) {
        return $http({ url: '/Account/LoginMethod', contentType: 'application/json; charset=utf-8', method: 'POST', data: JSON.stringify(registerModel), DataType: 'Json' });
    }
    UserService.reverifyAccountData = function (forgetModel) {
        return $http({ url: '/Account/ReverifyAccountMail', contentType: 'application/json; charset=utf-8', method: 'POST', data: JSON.stringify(forgetModel), DataType: 'Json' });
    }
    UserService.forgotData = function (forgetModel) {
        return $http({ url: '/Account/ForgotPasswordMail', contentType: 'application/json; charset=utf-8', method: 'POST', data: JSON.stringify(forgetModel), DataType: 'Json' });
    }
    UserService.ResetPassword = function (registerModel) {
        return $http({ url: '/Account/ResetPassword', contentType: 'application/json; charset=utf-8', method: 'POST', data: JSON.stringify(registerModel), DataType: 'Json' })
    }
    UserService.VerifyAccount = function (registerModel) {
        return $http({ url: '/Account/VerifyAccountData', contentType: 'application/json; charset=utf-8', method: 'POST', data: JSON.stringify(registerModel), DataType: 'Json' })
    }
    UserService.Logout = function (logoutModel) {
        return $http({ url: '/Account/Logout', contentType: 'application/json; charset=utf-8', method: 'POST', data: JSON.stringify(logoutModel), DataType: 'Json' })
    }
    return UserService;
}]);
