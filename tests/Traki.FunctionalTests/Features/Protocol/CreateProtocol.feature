 
Feature: Create protocol

Scenario: Create protocol with valid fields
    Given I have logged in as project manager
    And I have navigated to protocol templates page
    When I enter new protocol name and submit
    Then protocol should be added to the list