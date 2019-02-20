angular.module('Property').controller('RegisterController', function ($scope, $location, sweetAlert, UserService) {
    $scope.clear = function () {
        $scope.registrationModel = { UserName :'', FirstName: '', LastName: '', Email: '', Password: '', ConfirmPassword: '' };
    };

    $scope.doBlur = function ($event) {
        if ($event.keyCode === 13)
        {
            var target = $event.target;
            target.blur();
        }
    }

    $scope.clickRegister = function () {
        if ($scope.registrationForm.$valid) {
            $scope.laddaLoading = true;
            
            UserService.saveUserdata($scope.registrationModel)
                    .success(function (response) {
                        $scope.laddaLoading = false;
                        if (response.Status === "success") {
                            sweetAlert.swal({ title: "Yeah!", text: response.Message, type: "success" });
                            
                            $scope.clear();
                            $scope.registrationForm.confirmPassword.$setPristine();
                            $scope.registrationForm.confirmPassword.$setValidity();
                            $scope.registrationForm.confirmPassword.$setUntouched();

                            //$scope.doBlur($scope.registrationForm.confirmPassword);
                            $scope.registrationForm.$setPristine();
                            $scope.registrationForm.$setValidity();
                            $scope.registrationForm.$setUntouched();
                            //debugger;
                        }
                        else {
                            //Error in registration after service hit
                            sweetAlert.swal({ title: "Oops!", text: response.Message, type: "error" });
                        }
                    })
                    .error(function (user) {
                        //Error in registration
                        $scope.laddaLoading = false;
                        sweetAlert.swal({ title: "Oops!", text: response.Message, type: "error" });
                    })
        }
        else {
            angular.forEach($scope.registrationForm.$error.required, function (field) {
                field.$setDirty();
                field.$setTouched();
            });
        }
    };
});
