@ignore
Feature: Create defect

Scenario: Create defect without without uploading image
    Given I have logged in as project manager
    When I press on Projects tab
    Then projects should be displayed