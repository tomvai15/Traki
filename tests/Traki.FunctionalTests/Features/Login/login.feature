Feature: User authentication

Scenario: Log in with invalid credentials
    Given I have navigated to login page
    When I enter wrong credentials
    Then error message should be presented

@ignore
Scenario: Log in as product manager
    Given I have navigated to login page
    When I enter valid credentials
    Then I should be redirected to home page