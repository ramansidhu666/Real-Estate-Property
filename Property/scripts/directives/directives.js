/**
 * Property
  *
 */

angular.module('Property')
    .directive(['$rootScope', '$timeout', 'pageTitle', pageTitle])
    .directive(['$timeout','sideNavigation', sideNavigation])
    .directive(['$rootScope','minimalizaMenu', minimalizaMenu])
    .directive(['$timeout', 'sparkline', sparkline])
    .directive(['$timeout','icheck', icheck])
    .directive(['$timeout','panelTools', panelTools])
    .directive(['$timeout','panelToolsFullscreen', panelToolsFullscreen])
    .directive(['smallHeader', smallHeader])
    .directive(['$rootScope', '$timeout', 'animatePanel', animatePanel])
    .directive(['landingScrollspy', landingScrollspy])
    .directive(['clockPicker', clockPicker])
    .directive(['$parse','dateTimePicker', dateTimePicker])
    .directive(['$rootScope', '$timeout','dateRangePickerBasic', dateRangePickerBasic]) //dateRangePickerBasic
    .directive(['$rootScope', '$timeout','dateRangePicker', dateRangePicker]) //dateRangePicker
    .directive(['validPasswordCompare', validPasswordCompare]) //validPasswordCompare
    .directive(['$timeout', '$rootScope','syncFocusWith', syncFocusWith]) //syncFocusWith

/**
 * pageTitle - Directive for set Page title - my title
 */
function pageTitle($rootScope, $timeout) {
    return {
        link: ['$scope', function ($scope, element) {
            var listener = function (event, toState, toParams, fromState, fromParams) {

                $timeout(function () {
                    $scope.getTitle = function (name) {
                        $rootScope.$broadcast('getWordData', { key: name });
                        if (angular.isDefined($rootScope.titleName)) {
                            return $rootScope.titleName;
                        }
                        return name;
                    };

                    // Default title
                    var title = 'Property | Home';
                    // Create your own title pattern
                    if (toState.data && toState.data.pageTitle) title = 'Property | ' + $scope.getTitle(toState.data.pageTitle);

                    element.text(title);
                });
            };
            $rootScope.$on('$stateChangeStart', listener);
        }]
    }
};

/**
 * sideNavigation - Directive for run metsiMenu on sidebar navigation
 */
function sideNavigation($timeout) {
    return {
        restrict: 'A',
        link: function (scope, element) {
            // Call the metsiMenu plugin and plug it to sidebar navigation
            element.metisMenu();

            // Colapse menu in mobile mode after click on element
            var menuElement = $('#side-menu a:not([href$="\\#"])');
            menuElement.click(function () {

                if ($(window).width() < 769) {
                    $("body").toggleClass("show-sidebar");
                }
            });


        }
    };
};

/**
 * minimalizaSidebar - Directive for minimalize sidebar
 */
function minimalizaMenu($rootScope) {
    return {
        restrict: 'EA',
        template: '<div class="header-link hide-menu" ng-click="minimalize()"><i class="fa fa-bars"></i></div>',
        controller: ['$scope', function ($scope, $element) {

            $scope.minimalize = function () {
                if ($(window).width() < 769) {
                    $("body").toggleClass("show-sidebar");
                } else {
                    $("body").toggleClass("hide-sidebar");
                }
            }
        }]
    };
};


/**
 * sparkline - Directive for Sparkline chart
 */
function sparkline($timeout) {
    return {
        restrict: 'A',
        scope: {
            sparkData: '=',
            sparkOptions: '=',
        },
        link: function (scope, element, attrs) {
            return $timeout(function () {
                scope.$watch(scope.sparkData, function () {
                    render();
                });
                scope.$watch(scope.sparkOptions, function () {
                    render();
                });
                var render = function () {
                    $(element).sparkline(scope.sparkData, scope.sparkOptions);
                };
            });
        }
    }
};

/**
 * icheck - Directive for custom checkbox icheck
 */
function icheck($timeout) {
    return {
        restrict: 'A',
        require: 'ngModel',
        link: ['$scope', 'element', '$attrs', 'ngModel',function ($scope, element, $attrs, ngModel) {
            return $timeout(function () {
                var value;
                value = $attrs['value'];

                $scope.$watch($attrs['ngModel'], function (newValue) {
                    $(element).iCheck('update');
                })

                return $(element).iCheck({
                    checkboxClass: 'icheckbox_square-green',
                    radioClass: 'iradio_square-green'

                }).on('ifChanged', function (event) {
                    if ($(element).attr('type') === 'checkbox' && $attrs['ngModel']) {
                        $scope.$apply(function () {
                            return ngModel.$setViewValue(event.target.checked);
                        });
                    }
                    if ($(element).attr('type') === 'radio' && $attrs['ngModel']) {
                        return $scope.$apply(function () {
                            return ngModel.$setViewValue(value);
                        });
                    }
                });
            });
        }]
    };
}


/**
 * panelTools - Directive for panel tools elements in right corner of panel
 */
function panelTools($timeout) {
    return {
        restrict: 'A',
        scope: true,
        templateUrl: 'app-views/common/panel_tools.html',
        controller: ['$scope', '$element',function ($scope, $element) {
            // Function for collapse ibox
            $scope.showhide = function () {
                var hpanel = $element.closest('div.hpanel');
                var icon = $element.find('i:first');
                var body = hpanel.find('div.panel-body');
                var footer = hpanel.find('div.panel-footer');
                body.slideToggle(300);
                footer.slideToggle(200);
                // Toggle icon from up to down
                icon.toggleClass('fa-chevron-up').toggleClass('fa-chevron-down');
                hpanel.toggleClass('').toggleClass('panel-collapse');
                $timeout(function () {
                    hpanel.resize();
                    hpanel.find('[id^=map-]').resize();
                }, 50);
            },

            // Function for close ibox
            $scope.closebox = function () {
                var hpanel = $element.closest('div.hpanel');
                hpanel.remove();
            }

        }]
    };
};

/**
 * panelToolsFullscreen - Directive for panel tools elements in right corner of panel with fullscreen option
 */
function panelToolsFullscreen($timeout) {
    return {
        restrict: 'A',
        scope: true,
        templateUrl: 'app-views/common/panel_tools_fullscreen.html',
        controller:['$scope', '$element', function ($scope, $element) {
            // Function for collapse ibox
            $scope.showhide = function () {
                var hpanel = $element.closest('div.hpanel');
                var icon = $element.find('i:first');
                var body = hpanel.find('div.panel-body');
                var footer = hpanel.find('div.panel-footer');
                body.slideToggle(300);
                footer.slideToggle(200);
                // Toggle icon from up to down
                icon.toggleClass('fa-chevron-up').toggleClass('fa-chevron-down');
                hpanel.toggleClass('').toggleClass('panel-collapse');
                $timeout(function () {
                    hpanel.resize();
                    hpanel.find('[id^=map-]').resize();
                }, 50);
            };

            // Function for close ibox
            $scope.closebox = function () {
                var hpanel = $element.closest('div.hpanel');
                hpanel.remove();
                if ($('body').hasClass('fullscreen-panel-mode')) { $('body').removeClass('fullscreen-panel-mode'); }
            };

            // Function for fullscreen
            $scope.fullscreen = function () {
                var hpanel = $element.closest('div.hpanel');
                var icon = $element.find('i:first');
                $('body').toggleClass('fullscreen-panel-mode');
                icon.toggleClass('fa-expand').toggleClass('fa-compress');
                hpanel.toggleClass('fullscreen');
                setTimeout(function () {
                    $(window).trigger('resize');
                }, 100);
            }

        }]
    };
};

/**
 * smallHeader - Directive for page title panel
 */
function smallHeader() {
    return {
        restrict: 'A',
        scope: true,
        controller:['$scope', '$element', function ($scope, $element) {
            $scope.small = function () {
                var icon = $element.find('i:first');
                var breadcrumb = $element.find('#hbreadcrumb');
                $element.toggleClass('small-header');
                breadcrumb.toggleClass('m-t-lg');
                icon.toggleClass('fa-arrow-up').toggleClass('fa-arrow-down');
            }
        }]
    }
}

function animatePanel($timeout, $state) {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {

            //Set defaul values for start animation and delay
            var startAnimation = 0;
            var delay = 0.06;   // secunds
            var start = Math.abs(delay) + startAnimation;

            // Store current state where directive was start
            var currentState = $state.current.name;

            // Set default values for attrs
            if (!attrs.effect) { attrs.effect = 'zoomIn' };
            if (attrs.delay) { delay = attrs.delay / 10 } else { delay = 0.06 };
            if (!attrs.child) { attrs.child = '.row > div' } else { attrs.child = "." + attrs.child };

            // Get all visible element and set opactiy to 0
            var panel = element.find(attrs.child);
            panel.addClass('opacity-0');

            // Count render time
            var renderTime = panel.length * delay * 1000 + 700;

            // Wrap to $timeout to execute after ng-repeat
            $timeout(function () {

                // Get all elements and add effect class
                panel = element.find(attrs.child);
                panel.addClass('stagger').addClass('animated-panel').addClass(attrs.effect);

                var panelsCount = panel.length + 10;
                var animateTime = (panelsCount * delay * 10000) / 10;

                // Add delay for each child elements ninety thousand
                panel.each(function (i, elm) {
                    start += delay;
                    var rounded = Math.round(start * 10) / 10;
                    $(elm).css('animation-delay', rounded + 's');
                    // Remove opacity 0 after finish
                    $(elm).removeClass('opacity-0');
                });

                // Clear animation after finish
                $timeout(function () {
                    $('.stagger').css('animation', '');
                    $('.stagger').removeClass(attrs.effect).removeClass('animated-panel').removeClass('stagger');
                    panel.resize();
                }, animateTime)

            });



        }
    }
}

function landingScrollspy() {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            element.scrollspy({
                target: '.navbar-fixed-top',
                offset: 80
            });
        }
    }
}

/**
 * clockPicker - Directive for clock picker plugin
 */
function clockPicker() {
    return {
        restrict: 'A',
        link: function (scope, element) {
            element.clockpicker();
        }
    };
};

function dateTimePicker($parse) {
    return {
        require: '?ngModel',
        restrict: 'AE',
        scope: {
            locale: '@',
        },
        link: ['$scope', 'elem', '$attrs', 'ngModel', function ($scope, elem, $attrs, ngModel) {
            if (!ngModel) return; // do nothing if no ng-model
            ngModel.$render = function () {
                elem.find('input').val(ngModel.$viewValue || '');
            }

            elem.datetimepicker({
                format: 'MM/DD/YYYY',
                locale: $scope.locale
            });

            elem.on('dp.change', function () {
                $scope.$apply(read);
            });

            $(elem.find('input')).on('blur', function () {
                $scope.$apply(read);
            });

            function read() {
                var value = elem.find('input').val();
                ngModel.$setViewValue(value);
            }
        }]
    };
}

/**
 * dateRangePickerBasic - Directive for Date Range Picker Basic
 * http://www.jqueryscript.net/demo/Multi-language-jQuery-Date-Range-Picker-Plugin/
 */
function dateRangePickerBasic($rootScope, $timeout) {
    return {
        restrict: 'A',
        scope: {
            openDirection: '@',
            bindto: '@'
        },
        link:['$scope', '$elem', '$attrs', function ($scope, $elem, $attrs) {
            $timeout(function () {
                $scope.getTitle = function (name) {
                    $rootScope.$broadcast('getWordData', { key: name });
                    if (angular.isDefined($rootScope.titleName)) {
                        return $rootScope.titleName;
                    }
                    return name;
                };
                $scope.loadData = function () {
                    var todayDate = moment();
                    var start = moment().subtract(29, 'days');
                    var end = moment();
                    var monthNameList = [$scope.getTitle('January'), $scope.getTitle('February'), $scope.getTitle('March'), $scope.getTitle('April'), $scope.getTitle('May'), $scope.getTitle('June'), $scope.getTitle('July'), $scope.getTitle('August'), $scope.getTitle('September'), $scope.getTitle('October'), $scope.getTitle('November'), $scope.getTitle('December')];
                    var daysOfWeekList = [$scope.getTitle('Sunday').substring(0, 2), $scope.getTitle('Monday').substring(0, 2), $scope.getTitle('Tuesday').substring(0, 2), $scope.getTitle('Wednesday').substring(0, 2), $scope.getTitle('Thusday').substring(0, 2), $scope.getTitle('Friday').substring(0, 2), $scope.getTitle('Saturday').substring(0, 2)];
                    function cb1(start, end) {
                        $('#' + $scope.bindto + ' span').html(start.format('DD.MM.YYYY') + ' - ' + end.format('DD.MM.YYYY'));
                    }
                    $('#' + $scope.bindto).daterangepicker({
                        startDate: start,
                        endDate: end,
                        opens: $scope.openDirection,
                        locale: {
                            "format": 'DD.MM.YYYY',
                            "separator": ' - ',
                            "applyLabel": $scope.getTitle('Apply'),
                            "cancelLabel": $scope.getTitle('Cancel'),
                            "fromLabel": $scope.getTitle('From'),
                            "toLabel": $scope.getTitle('To'),
                            "customRangeLabel": $scope.getTitle('Custom'),
                            "daysOfWeek": daysOfWeekList,
                            "monthNames": monthNameList,
                        }
                    }, cb1);
                    cb1(start, end);
                };
                $scope.loadData();

                $scope.$on('changeDateRange', function (event, obj) {
                    //if (obj.indexValue == 1) {
                    //    scope.myData1 = obj.option;
                    //}
                    //else {
                    //    scope.myData2 = obj.option;
                    //}
                    $scope.loadData();
                });

            });
        }]
    };
};

/**
 * dateRangePicker - Directive for Date Range Picker
 * http://www.jqueryscript.net/demo/Multi-language-jQuery-Date-Range-Picker-Plugin/
 */
function dateRangePicker($rootScope, $timeout) {
    return {
        require: '?ngModel',
        restrict: 'AE',
        scope: {
            openDirection: '@',
            bindto: '@',
            defaultDateOption: '@',
            dateFormat: '@'
        },
        link:['$scope', '$elem', '$attrs', 'ngModel', function ($scope, $elem, $attrs, ngModel) {
            $timeout(function () {
                $scope.getTitle = function (name) {
                    $rootScope.$broadcast('getWordData', { key: name });
                    if (angular.isDefined($rootScope.titleName)) {
                        return $rootScope.titleName;
                    }
                    return name;
                };

                $scope.loadData = function () {
                    var todayDate = moment();
                    var start = moment().subtract(29, 'days');
                    var end = moment();
                    var todayLabel = 'Today', yesterdayLabel = 'Yesterday', last7DaysLabel = 'Last 7 Days', last30DaysLabel = 'Last 30 Days', thisMonthLabel = 'This Month', lastMonthLabel = 'Last Month';

                    todayLabel = $scope.getTitle(todayLabel), yesterdayLabel = $scope.getTitle(yesterdayLabel), last7DaysLabel = $scope.getTitle(last7DaysLabel), last30DaysLabel = $scope.getTitle(last30DaysLabel), thisMonthLabel = $scope.getTitle(thisMonthLabel), lastMonthLabel = $scope.getTitle(lastMonthLabel);
                    var rangesVal = '{"' + todayLabel + '":[' + todayDate + ',' + todayDate + '],';
                    rangesVal += '"' + yesterdayLabel + '":[' + moment().subtract(1, 'days') + ',' + moment().subtract(1, 'days') + '],';
                    rangesVal += '"' + last7DaysLabel + '":[' + moment().subtract(6, 'days') + ',' + todayDate + '],';
                    rangesVal += '"' + last30DaysLabel + '":[' + moment().subtract(29, 'days') + ',' + todayDate + '],';
                    rangesVal += '"' + thisMonthLabel + '":[' + moment().startOf('month') + ',' + moment().endOf('month') + '],';
                    rangesVal += '"' + lastMonthLabel + '":[' + moment().subtract(1, 'month').startOf('month') + ',' + moment().subtract(1, 'month').endOf('month') + ']}';

                    var monthNameList = [$scope.getTitle('January'), $scope.getTitle('February'), $scope.getTitle('March'), $scope.getTitle('April'), $scope.getTitle('May'), $scope.getTitle('June'), $scope.getTitle('July'), $scope.getTitle('August'), $scope.getTitle('September'), $scope.getTitle('October'), $scope.getTitle('November'), $scope.getTitle('December')];
                    var daysOfWeekList = [$scope.getTitle('Sunday').substring(0, 2), $scope.getTitle('Monday').substring(0, 2), $scope.getTitle('Tuesday').substring(0, 2), $scope.getTitle('Wednesday').substring(0, 2), $scope.getTitle('Thusday').substring(0, 2), $scope.getTitle('Friday').substring(0, 2), $scope.getTitle('Saturday').substring(0, 2)];
                    function cb1(start, end) {
                        $('#' + $scope.bindto + ' span').html(start.format($scope.dateFormat) + ' - ' + end.format($scope.dateFormat));
                    }
                    if ($scope.defaultDateOption == 'Today') {
                        start = moment();
                        end = moment();
                    }
                    else if ($scope.defaultDateOption == 'Yesterday') {
                        start = moment().subtract(1, 'days');
                        end = moment().subtract(1, 'days');
                    }
                    else if ($scope.defaultDateOption == 'Last7Days') {
                        start = moment().subtract(6, 'days');
                        end = moment();
                    }
                    else if ($scope.defaultDateOption == 'Last30Days') {
                        start = moment().subtract(29, 'days');
                        end = moment();
                    }
                    else if ($scope.defaultDateOption == 'ThisMonth') {
                        start = moment().startOf('month');
                        end = moment().endOf('month');
                    }
                    else if ($scope.defaultDateOption == 'LastMonth') {
                        start = moment().subtract(1, 'month').startOf('month');
                        end = moment().subtract(1, 'month').endOf('month');
                    }

                    $('#' + $scope.bindto).daterangepicker({
                        startDate: start,
                        endDate: end,
                        opens: $scope.openDirection,
                        ranges: angular.fromJson(rangesVal),
                        locale: {
                            "format": $scope.dateFormat,
                            "separator": ' - ',
                            "applyLabel": $scope.getTitle('Apply'),
                            "cancelLabel": $scope.getTitle('Cancel'),
                            "fromLabel": $scope.getTitle('From'),
                            "toLabel": $scope.getTitle('To'),
                            "customRangeLabel": $scope.getTitle('Custom'),
                            "daysOfWeek": daysOfWeekList,
                            "monthNames": monthNameList,
                        }
                    }, cb1);
                    cb1(start, end);

                    $scope.$apply(read(start, end));
                };
                $scope.loadData();

                $('#' + $scope.bindto).on('apply.daterangepicker', function (ev, picker) {
                    $scope.startDateSelected = picker.startDate.format($scope.dateFormat);
                    $scope.endDateSelected = picker.endDate.format($scope.dateFormat);
                    $scope.$apply(read(picker.startDate, picker.endDate));
                });
                $('#' + $scope.bindto).on('cancel.daterangepicker', function (ev, picker) {
                    //$(this).val('');
                });
                function read(startDate, endDate) {
                    var value = { startDate: startDate, endDate: endDate };
                    ngModel.$setViewValue(value);
                }
            });
        }]
    };
};

function validPasswordCompare() {
    return {
        require: 'ngModel',
        scope: {
            reference: '=validPasswordCompare'
        },
        link: function (scope, elm, attrs, ctrl) {
            ctrl.$parsers.unshift(function (viewValue, $scope) {
                var noMatch = viewValue != scope.reference
                ctrl.$setValidity('noMatch', !noMatch);
                return (noMatch) ? noMatch : !noMatch;
            });
            scope.$watch("reference", function (value) {;
                ctrl.$setValidity('noMatch', value === ctrl.$viewValue);
            });
        }
    }
};

function syncFocusWith($timeout, $rootScope) {
    return {
        restrict: 'A',
        scope: {
            focusValue: "=syncFocusWith"
        },
        link: ['$scope', '$element', 'attrs',function ($scope, $element, attrs) {
            //debugger;
            $scope.$watch("focusValue", function (currentValue, previousValue) {
                //debugger;
                if (currentValue === true && !previousValue) {
                    $element[0].focus();
                } else if (currentValue === false && previousValue) {
                    $element[0].blur();
                }
            })
        }]
    }
};


