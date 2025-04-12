using Duo.Services;
using Duo.ViewModels;
using Duo.ViewModels.CreateExerciseViewModels;
using Duo.ViewModels.ExerciseViewModels;
using DuoTesting.Services;
using Microsoft.UI.Xaml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.AppContainer;
using Moq;

namespace DuoTesting;


[TestClass]
public class ExerciseTemplateSelectorUT
{
    private ExerciseTemplateSelector _selector;
    private DataTemplate _associationTemplate;
    private DataTemplate _fillInTheBlankTemplate;
    private DataTemplate _multipleChoiceTemplate;
    private DataTemplate _flashcardTemplate;

    //mock exercise service with moq
    private Mock<IExerciseService> _mockExerciseService;

    //mock viewmodels with moq
    private Mock<AssociationExerciseViewModel> _mockAssociationExerciseViewModel;
    private Mock<FillInTheBlankExerciseViewModel> _mockFillInTheBlankExerciseViewModel;
    private Mock<MultipleChoiceExerciseViewModel> _mockMultipleChoiceExerciseViewModel;
    private Mock<CreateFlashcardExerciseViewModel> _mockCreateFlashcardExerciseViewModel;

    
    [UITestMethod]
    public void SelectTemplate_AssociationExerciseViewModel_ReturnsAssociationTemplate()
    {
        _associationTemplate = new DataTemplate();
        _fillInTheBlankTemplate = new DataTemplate();
        _multipleChoiceTemplate = new DataTemplate();
        _flashcardTemplate = new DataTemplate();
        _mockExerciseService = new Mock<IExerciseService>();

        _selector = new ExerciseTemplateSelector
        {
            AssociationExerciseTemplate = _associationTemplate,
            FillInTheBlankExerciseTemplate = _fillInTheBlankTemplate,
            MultipleChoiceExerciseTemplate = _multipleChoiceTemplate,
            FlashcardExerciseTemplate = _flashcardTemplate
        };

        var vm = new AssociationExerciseViewModel(_mockExerciseService.Object);
        var result = _selector.SelectTemplate(vm);
        Assert.AreEqual(_associationTemplate, result);
    }

    [UITestMethod]
    public void SelectTemplate_FillInTheBlankExerciseViewModel_ReturnsFillInTheBlankTemplate()
    {
        _associationTemplate = new DataTemplate();
        _fillInTheBlankTemplate = new DataTemplate();
        _multipleChoiceTemplate = new DataTemplate();
        _flashcardTemplate = new DataTemplate();
        _mockExerciseService = new Mock<IExerciseService>();

        _selector = new ExerciseTemplateSelector
        {
            AssociationExerciseTemplate = _associationTemplate,
            FillInTheBlankExerciseTemplate = _fillInTheBlankTemplate,
            MultipleChoiceExerciseTemplate = _multipleChoiceTemplate,
            FlashcardExerciseTemplate = _flashcardTemplate
        };

        var vm = new FillInTheBlankExerciseViewModel(_mockExerciseService.Object);
        var result = _selector.SelectTemplate(vm);
        Assert.AreEqual(_fillInTheBlankTemplate, result);
    }

    [UITestMethod]
    public void SelectTemplate_MultipleChoiceExerciseViewModel_ReturnsMultipleChoiceTemplate()
    {
        _associationTemplate = new DataTemplate();
        _fillInTheBlankTemplate = new DataTemplate();
        _multipleChoiceTemplate = new DataTemplate();
        _flashcardTemplate = new DataTemplate();
        _mockExerciseService = new Mock<IExerciseService>();

        _selector = new ExerciseTemplateSelector
        {
            AssociationExerciseTemplate = _associationTemplate,
            FillInTheBlankExerciseTemplate = _fillInTheBlankTemplate,
            MultipleChoiceExerciseTemplate = _multipleChoiceTemplate,
            FlashcardExerciseTemplate = _flashcardTemplate
        };

        var vm = new MultipleChoiceExerciseViewModel(_mockExerciseService.Object);
        var result = _selector.SelectTemplate(vm);
        Assert.AreEqual(_multipleChoiceTemplate, result);
    }

    [UITestMethod]
    public void SelectTemplate_CreateFlashcardExerciseViewModel_ReturnsFlashcardTemplate()
    {
        _associationTemplate = new DataTemplate();
        _fillInTheBlankTemplate = new DataTemplate();
        _multipleChoiceTemplate = new DataTemplate();
        _flashcardTemplate = new DataTemplate();
        _mockExerciseService = new Mock<IExerciseService>();

        _selector = new ExerciseTemplateSelector
        {
            AssociationExerciseTemplate = _associationTemplate,
            FillInTheBlankExerciseTemplate = _fillInTheBlankTemplate,
            MultipleChoiceExerciseTemplate = _multipleChoiceTemplate,
            FlashcardExerciseTemplate = _flashcardTemplate
        };

        var vm = new CreateFlashcardExerciseViewModel();
        var result = _selector.SelectTemplate(vm);
        Assert.AreEqual(_flashcardTemplate, result);
    }

    [UITestMethod]
    public void SelectTemplate_UnknownType_ReturnsNull()
    {
        _associationTemplate = new DataTemplate();
        _fillInTheBlankTemplate = new DataTemplate();
        _multipleChoiceTemplate = new DataTemplate();
        _flashcardTemplate = new DataTemplate();
        _mockExerciseService = new Mock<IExerciseService>();

        _selector = new ExerciseTemplateSelector
        {
            AssociationExerciseTemplate = _associationTemplate,
            FillInTheBlankExerciseTemplate = _fillInTheBlankTemplate,
            MultipleChoiceExerciseTemplate = _multipleChoiceTemplate,
            FlashcardExerciseTemplate = _flashcardTemplate
        };

        var result = _selector.SelectTemplate(new object());
        Assert.IsNull(result);
    }
}
