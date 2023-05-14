@ignore
Feature: Update section

Scenario: Update section with valid fields
    Given I have logged in as project manager
    And I have navigated to projects page
    When I open edit project page
    And I update all project fields
    Then project information should be updated

Scenario: Update section with invalid fields
    Given I have logged in as project manager
    And I have navigated to projects page
    When I open edit project page
    And I update all project fields with invalid characters
    Then I should not be allowed to update project information