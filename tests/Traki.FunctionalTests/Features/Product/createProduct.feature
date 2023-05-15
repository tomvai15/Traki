 
Feature: Create product

Scenario: Create product with valid fields
    Given I have logged in as product manager
    And I have navigated to projects page
    When I open create product page
    And I add product name
    Then product is created