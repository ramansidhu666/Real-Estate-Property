/**
 * Property Config
 * version 1.0
 *
 */

function configState($stateProvider, $urlRouterProvider, $compileProvider) {

    // Optimize load start with remove binding information inside the DOM element
    $compileProvider.debugInfoEnabled(true);

    // Set default state 
    $urlRouterProvider.otherwise("/");

    $stateProvider
        .state('loginOn', {
            url: "/",
            templateUrl: null,
            controller: ['$scope', '$stateParams', '$rootScope', '$location', '$state', '$http', function ($scope, $stateParams, $rootScope, $location, $state, $http) {
                if ($rootScope.currentUrl === "resetpassword" || $rootScope.currentUrl === "verifyaccount") {
                    //Do Nothing
                }
                else {
                    $state.go("login", {}, { reload: true })
                }
            }],
            data: {
                pageTitle: ''
            }
        })
        //Login
        .state('login', {
            url: "/login",
            templateUrl: "account/login", //controller/action
            data: {
                pageTitle: 'Login',
                specialClass: 'blank'
            }
        })
        //Login - Header partial view
        .state('login_header', {
            url: "/login_header",
            templateUrl: "partial/loginheader", //controller/action
        })
        //Login - Footer partial view
        .state('login_footer', {
            url: "/login_footer",
            templateUrl: "partial/loginfooter", //controller/action
        })
        //Page Not Found
        .state('pagenotfound', {
            url: "/pagenotfound",
            templateUrl: "partial/pagenotfound", //controller/action
            data: {
                pageTitle: '404-Page not found',
                specialClass: 'blank'
            }
        })
        //Page Not Found
        .state('pageexpire', {
            url: "/pageexpire",
            templateUrl: "partial/pageexpire", //controller/action
            data: {
                pageTitle: 'Link Expired',
                specialClass: 'blank'
            }
        })
        //Register
        .state('register', {
            url: "/register",
            templateUrl: "account/register", //controller/action
            data: {
                pageTitle: 'Register',
                specialClass: 'blank'
            }
        })
         //Verify Account
        .state('verifyaccount', {
            url: "/verifyaccount",
            templateUrl: "account/verifyaccount", //controller/action
            data: {
                pageTitle: 'Verify Account',
                specialClass: 'blank'
            }
        })
         //Re-Verify Account
        .state('reverifyaccount', {
            url: "/reverifyaccount",
            templateUrl: "account/reverifyaccount", //controller/action
            data: {
                pageTitle: 'Re-Verify Account',
                specialClass: 'blank'
            }
        })
        //Forgot Password
        .state('forgotpassword', {
            url: "/forgotpassword",
            templateUrl: "account/forgotpassword", //controller/action
            data: {
                pageTitle: 'Forgot Password',
            }
        })
         //Reset Password
        .state('resetpassword', {
            url: "/resetpassword",
            templateUrl: "account/resetpassword", //controller/action
            data: {
                pageTitle: 'Reset Password',
            }
        })  
        // Dashboard - Home
        .state('dashboard', {
            url: "/dashboard",
            templateUrl: "dashboard/home", //controller/action
            data: {
                pageTitle: 'Dashboard'
            }
        })
        // Dashboard - Base Main page
        .state('dashboard.base', {
            url: "/dashboardbase",
            templateUrl: "dashboard/index", //controller/action
            data: {
                pageTitle: 'Dashboard'
            }
        })
        // Dashboard - Header page partial view
        .state('dashboard_header', {
            url: "/dashboardheader",
            templateUrl: "dashboard/dashboardHeader", //controller/action
            data: {
            }
        })
        // Dashboard - Navigation partial view
        .state('dashboard_navigation', {
            url: "/navigation",
            templateUrl: "dashboard/navigation", //controller/action
            data: {
            }
        })
        // Dashboard - Page page partial view
        .state('page_header', {
            url: "/pagedheader",
            templateUrl: "dashboard/pageHeader", //controller/action
            data: {
            }
        })
}

angular.module('Property').config(['$stateProvider', '$urlRouterProvider', '$compileProvider', configState])
    .run(['$rootScope', '$state', '$location', 'editableOptions', function ($rootScope, $state, $location, editableOptions) {
        $rootScope.$state = $state;
        editableOptions.theme = 'bs3';
        $rootScope.$on('$stateChangeStart', function (event, toState, toParams, fromState, fromParams) {
            $("#ui-view").html("");
            $('.splash').css('display', 'block');
        });

        $rootScope.$on('$stateChangeSuccess', function (event, toState, toParams, fromState, fromParams) {
            $('.splash').css('display', 'none');
        });

        $rootScope.$on('$routeChangeStart', function () {
            ('.splash').css('display', 'block');
        });
        $rootScope.$on('$routeChangeSuccess', function () {
            $('.splash').css('display', 'none');
        });
    }]);
