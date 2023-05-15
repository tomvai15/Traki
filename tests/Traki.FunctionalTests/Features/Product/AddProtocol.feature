 
Feature: Add protocol

Scenario: Add protocol for product
    Given I have logged in as product manager
    And I have navigated to projects page
    And I have opened product page
    When I open protocol import window
    And select protocol from the list
    Then protocol should be added for product