Feature: View project

Scenario: Open project tab, shows all projects
    Given I have logged in as project manager
    When I press on Projects tab
    Then projects should be displayed