@ignore
Feature: Delete product

Scenario: Delete product
    Given I have logged in as product manager
    And I have navigated to projects page
    And I have created product
    When I open edit product page
    And press delete button and confirm deletion
    Then product is deleted