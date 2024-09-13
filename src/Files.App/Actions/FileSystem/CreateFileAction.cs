using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Files.App.Actions.FileSystem
{
	internal sealed class CreateFileAction : BaseUIAction, IAction
	{
		private readonly IContentPageContext context;


		public string Label
			=> "File".GetLocalizedResource();

		// TODO: localize
		public string Description
			=> "新しいファイルを作成する";

		public HotKey HotKey
			=> new(Keys.N, KeyModifiers.CtrlAlt);

		public RichGlyph Glyph
			=> new(baseGlyph: "\uE7C3");

		public override bool IsExecutable =>
			context.CanCreateItem &&
			UIHelpers.CanShowDialog;

		public CreateFileAction()
		{
			context = Ioc.Default.GetRequiredService<IContentPageContext>();

			context.PropertyChanged += Context_PropertyChanged;
		}

		public Task ExecuteAsync(object? parameter = null)
		{
			if (context.ShellPage is not null)
				UIFilesystemHelpers.CreateFileFromDialogResultTypeAsync(AddItemDialogItemType.File, null!, context.ShellPage);

			return Task.CompletedTask;
		}

		private void Context_PropertyChanged(object? sender, PropertyChangedEventArgs e)
		{
			switch (e.PropertyName)
			{
				case nameof(IContentPageContext.CanCreateItem):
				case nameof(IContentPageContext.HasSelection):
					OnPropertyChanged(nameof(IsExecutable));
					break;
			}
		}
	}
}
