using System.Collections.Generic;
using TrackerDesktop.Data.Entities;
using TrackerDesktop.Models;

namespace TrackerDesktop.Mappers
{
    public class ExcelMappers
    {
        public static ExcelMode MapModeToExcelMode(Mode mode)
        {
            var excelMode = new ExcelMode
            {
                Id = mode.Id,
                Name = mode.Name,
                MaxBottleNumber = mode.MaxBottleNumber,
                MaxUsedTips = mode.MaxUsedTips,
            };

            return excelMode;
        }

        public static ExcelStep MapStepToExcelStep(Step step)
        {
            var execlStep = new ExcelStep
            {
                Id = step.Id,
                ModeId = step.ModeId,
                Timer = step.Timer,
                Destination = step.Destination,
                Speed = step.Speed,
                Type = step.Type,
                Volume = step.Volume,
            };

            return execlStep;
        }

        public static List<ExcelMode> MapModesToExcelModes(IEnumerable<Mode> modes)
        {
            var execelModes = new List<ExcelMode>();

            foreach (var mode in modes)
            {
                execelModes.Add(MapModeToExcelMode(mode));
            }

            return execelModes;
        }

        public static List<ExcelStep> MapStepsToExcelSteps(IEnumerable<Step> steps)
        {
            var excelSteps = new List<ExcelStep>();

            foreach (var step in steps)
            {
                excelSteps.Add(MapStepToExcelStep(step));
            }

            return excelSteps;
        }
    }
}
