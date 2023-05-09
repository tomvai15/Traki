Feature: Delete project

Scenario: Delete project with products
    Given I have logged in as project manager
    When I press on Projects tab
    Then projects should be displayed