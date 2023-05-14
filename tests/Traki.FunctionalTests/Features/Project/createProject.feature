@ignore
Feature: Create project

Scenario: Create project with valid fields
    Given I have logged in as project manager
    When I press on Projects tab
    Then projects should be displayed