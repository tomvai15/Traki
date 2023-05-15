 
Feature: Fill protocol

Scenario: Fill all protocol sections
    Given I have logged in as product manager
    And I have navigated to projects page
    And I have opened product page
    When I open fill protocol page
    And fill section and save changes
    Then section should be updated