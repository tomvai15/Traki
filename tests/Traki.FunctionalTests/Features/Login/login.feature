@ignore
Feature: User authentication

Scenario: Log in with invalid credentials
    When I enter wrong credentials
    Then error message should be presented

Scenario: Log in as product manager
    When I enter valid credentials
    Then I should be redirected to home page