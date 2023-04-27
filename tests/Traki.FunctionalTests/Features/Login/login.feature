@Login
Feature: Login features

Scenario: Incorrect login
    When I enter wrong credentials
    Then error message should be presented

Scenario: Valid login
    When I enter valid credentials
    Then I should be redirected to home page