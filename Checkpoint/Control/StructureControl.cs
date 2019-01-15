using Checkpoint.DAO;
using Checkpoint.Model;
using System;
using System.Collections.Generic;

namespace Checkpoint.Control
{
    class StructureControl
    {
        StructureDAO structureDAO = new StructureDAO();

        public Boolean saveStructure(Structure structure)
        {
            return structureDAO.saveStructure(structure);
        }

        public Boolean updateStructure(Structure structure)
        {
            return structureDAO.updateStructure(structure);
        }

        public Boolean deleteStructure(Structure structure)
        {
            return structureDAO.deleteStructure(structure);
        }

        public List<Structure> getAllStructures()
        {
            return structureDAO.getAllStructures();
        }

    }
}
