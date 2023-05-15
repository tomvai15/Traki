 
Feature: Delete protocol

Scenario: Delete protocol with sections
    Given I have logged in as project manager
    And I have navigated to protocol templates page
    And I have created protocol
    When I press delete button and confirm deletion
    Then protocol should be deleted