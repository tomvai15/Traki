Feature: Change defect status

Scenario: Change defect status when it is unfixed
    Given I have logged in as project manager
    When I press on Projects tab
    Then projects should be displayed