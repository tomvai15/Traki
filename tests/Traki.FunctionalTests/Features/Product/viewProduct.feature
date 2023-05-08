Feature: View product
@ignore
Scenario: Open project tab, shows all projects
    Given I have logged in as product manager
    And I have navigated to projects page
    When I press on product
    Then product information should be displayed