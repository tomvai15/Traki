@Login
Feature: Login features

Scenario: Succesfull login reidrect to home page
    When I enter my credentials
    Then I should be redirected to home page