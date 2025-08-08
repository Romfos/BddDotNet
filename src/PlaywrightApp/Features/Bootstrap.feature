Feature: Bootstrap

Scenario: checkout form validation test
    Given navigate to '@HomePageUrl'
    When set following values:
    | Name                  | Value      |
    | checkout > first name | first_name |
    | checkout > last name  | last_name  |
    And click on 'checkout > continue to checkout'
    Then should be visible:
    | Name                              |
    | checkout > username error message |
    And should have following values:
    | Name                              | Value                      |
    | checkout > username error message | Your username is required. |