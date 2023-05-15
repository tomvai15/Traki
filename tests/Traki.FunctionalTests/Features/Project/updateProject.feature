 
Feature: Update project

Scenario: Update project with valid fields
    Given I have logged in as project manager
    When I press on Projects tab
    Then projects should be displayed

Scenario: Update project with invalid fields
    Given I have logged in as project manager
    And I have navigated to projects page
    When I open edit project page
    And I update all project fields with invalid characters
    Then I should not be allowed to update project information