 
Feature: View defects

Scenario: Defects screen displays defects
    Given I have logged in as product manager
    And I have navigated to projects page
    And I have opened product page
    When I open defects page
    Then defects information is displayed